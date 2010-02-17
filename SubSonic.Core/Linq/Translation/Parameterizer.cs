// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)
//Original code created by Matt Warren: http://iqtoolkit.codeplex.com/Release/ProjectReleases.aspx?ReleaseId=19725


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using SubSonic.Linq.Structure;

namespace SubSonic.Linq.Translation
{
    /// <summary>
    /// Converts user arguments into named-value parameters
    /// </summary>
    public class Parameterizer : DbExpressionVisitor
    {
        protected Dictionary<object, NamedValueExpression> map = new Dictionary<object, NamedValueExpression>();
        protected Dictionary<Expression, NamedValueExpression> pmap = new Dictionary<Expression,NamedValueExpression>();
        protected int iParam = 0;

        protected Parameterizer()
        {
        }

        public static Expression Parameterize(Expression expression)
        {
            return new Parameterizer().Visit(expression);
        }

        protected override Expression VisitProjection(ProjectionExpression proj)
        {
            // don't parameterize the projector or aggregator!
            SelectExpression select = (SelectExpression)this.Visit(proj.Source);
            if (select != proj.Source) {
                return new ProjectionExpression(select, proj.Projector, proj.Aggregator);
            }
            return proj;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            if (c.Value != null && !IsNumeric(c.Value.GetType())) {
                NamedValueExpression nv;
                if (!this.map.TryGetValue(c.Value, out nv)) { // re-use same name-value if same value
                    string name = "p" + (iParam++);
                    nv = new NamedValueExpression(name, c);
                    this.map.Add(c.Value, nv);
                }
                return nv;
            }
            return c;
        }

        protected override Expression VisitParameter(ParameterExpression p) 
        {
            return this.GetNamedValue(p);
        }

        private Expression GetNamedValue(Expression e)
        {
            NamedValueExpression nv;
            if (!this.pmap.TryGetValue(e, out nv))
            {
                string name = "p" + (iParam++);
                nv = new NamedValueExpression(name, e);
                this.pmap.Add(e, nv);
            }
            return nv;
        }

        private bool IsNumeric(Type type)
        {
            switch (Type.GetTypeCode(type)) {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
        }
    }

    /// <summary>
    /// Parameterization for Oracle is the same as the base Parameterizer,
    ///  except that all constants are parameterized, not just strings.
    /// Not parameterizing all constants can severely hinder Oracle's
    ///  a bility to scale, because it will reparse the sql and generate
    ///  a new execution plan for each unique query. Having literals
    ///  in the queries causes them to not exactly match.
    /// This is configurable behavior within Oracle, but we should
    ///  assume that most devs don't have the ability to tweak the
    ///  Oracle query analyzer.
    /// </summary>
    public class OracleParameterizer : Parameterizer
    {
        public static Expression Parameterize(Expression expression)
        {
            return new OracleParameterizer().Visit(expression);
        }

        // this method is a copy/paste of the one in Paramterizer, but without the check for "IsNumeric".
        protected override Expression VisitConstant(ConstantExpression c)
        {
            if (c.Value != null)
            {
                NamedValueExpression nv;
                if (!this.map.TryGetValue(c.Value, out nv))
                { // re-use same name-value if same value
                    string name = "p" + (iParam++);
                    nv = new NamedValueExpression(name, c);
                    this.map.Add(c.Value, nv);
                }
                return nv;
            }
            return c;
        }
    }

    internal class NamedValueGatherer : DbExpressionVisitor
    {
        HashSet<NamedValueExpression> namedValues = new HashSet<NamedValueExpression>();

        private NamedValueGatherer()
        {
        }

        internal static ReadOnlyCollection<NamedValueExpression> Gather(Expression expr)
        {
            NamedValueGatherer gatherer = new NamedValueGatherer();
            gatherer.Visit(expr);
            return gatherer.namedValues.ToList().AsReadOnly();
        }

        protected override Expression VisitNamedValue(NamedValueExpression value)
        {
            this.namedValues.Add(value);
            return value;
        }
    }
}