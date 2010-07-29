using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubSonic.Tests.Unit.Linq.SqlStrings {
	public class OracleSelectTestsSql : ISelectTestsSql {
		public string All_With_SubQuery
		{
			get {
				return @"
				SELECT t0.Address,
					   t0.City,
					   t0.CompanyName,
					   t0.ContactName,
					   t0.Country,
					   t0.CustomerID,
					   t0.Region
				  FROM Customers t0
				 WHERE NOT EXISTS
				 (SELECT NULL
						  FROM Orders t1
						 WHERE ((t1.CustomerID = t0.CustomerID) AND
							   NOT (t1.OrderDate > to_date('2008-05-01T00:00:00','YYYY-MM-DD""T""HH24:MI:SS'))))
			";
			}
		}

		public string Any_With_Collection
		{
			get { return @"
				SELECT t0.Address,
					   t0.City,
					   t0.CompanyName,
					   t0.ContactName,
					   t0.Country,
					   t0.CustomerID,
					   t0.Region
				  FROM Customers t0
				 WHERE ((t0.CustomerID = 'TEST1') OR (t0.CustomerID = 'TEST2'))
			";}
		}

		public string Any_With_Collection_One_False
		{
			get { return @"
				select t0.address,
					   t0.city,
					   t0.companyname,
					   t0.contactname,
					   t0.country,
					   t0.customerid,
					   t0.region
				  from customers t0
				 where ((t0.customerid = 'ABCDE') or (t0.customerid = 'TEST1'))
			";}
		}

		public string Contains_Resolves_Literal
		{
			get { return @"
				select t0.address,
					   t0.city,
					   t0.companyname,
					   t0.contactname,
					   t0.country,
					   t0.customerid,
					   t0.region
				  from customers t0
				  where (t0.contactname like '%' || 'har' || '%')
			";}
		}

		public string Contains_With_LocalCollection_2_True
		{
			get { return @"
				select t0.address,
					   t0.city,
					   t0.companyname,
					   t0.contactname,
					   t0.country,
					   t0.customerid,
					   t0.region
				  from customers t0
				 where t0.customerid in ('TEST2', 'TEST1')
			";}
		}

		public string Contains_With_LocalCollection_OneFalse
		{
			get { return @"
				select t0.address,
					   t0.city,
					   t0.companyname,
					   t0.contactname,
					   t0.country,
					   t0.customerid,
					   t0.region
				  from customers t0
				 where t0.customerid in ('ABCDE', 'TEST1')
			";}
		}

		public string Contains_With_Subquery
		{
			get { return @"
				select t0.address,
					   t0.city,
					   t0.companyname,
					   t0.contactname,
					   t0.country,
					   t0.customerid,
					   t0.region
				  from customers t0
				 where t0.customerid in (select t1.customerid from orders t1)
			"; }
		}

		public string Count_Distinct
		{
			get { return "select distinct t0.city from customers t0"; }
		}

		public string Count_No_Args
		{
			get { return "select t0.orderid from orders t0"; }
		}

		public string Distinct_GroupBy
		{
			get { return @"
				select t0.customerid
				  from (select distinct t1.customerid,
										t1.orderdate,
										t1.orderid,
										t1.requireddate,
										t1.shippeddate
						  from orders t1) t0
				 group by t0.customerid
			"; }
		}

		public string Distinct_Should_Not_Fail
		{
			get { return @"
				select distinct t0.address,
								t0.city,
								t0.companyname,
								t0.contactname,
								t0.country,
								t0.customerid,
								t0.region
				from customers t0
			"; }
		}

		public string Distinct_Should_Return_11_For_Scalar_CustomerCity
		{
			get { return "select distinct t0.city from customers t0"; }
		}

		public string Distinct_Should_Return_69_For_Scalar_CustomerCity_Ordered
		{
			get { return "select distinct t0.city from customers t0 order by t0.city"; }
		}

		public string GroupBy_Basic
		{
            get { return @"SELECT t0.City 
                            FROM Customers AS t0
                            GROUP BY t0.City 
                          SELECT t0.Address, 
                                 t0.City, 
                                 t0.CompanyName, 
                                 t0.ContactName, 
                                 t0.Country, 
                                 t0.CustomerID, 
                                 t0.Region
                            FROM Customers AS t0
                            WHERE ((t0.City IS NULL AND t1.City IS NULL) 
                               OR (t0.City = t1.City))";
            }
		}

		public string GroupBy_Distinct
		{
			get { return ""; }
		}

		public string GroupBy_SelectMany
		{
			get { return ""; }
		}

		public string GroupBy_Sum
		{
			get { return ""; }
		}

		public string GroupBy_Sum_With_Element_Selector_Sum_Max
		{
			get { return ""; }
		}

		public string GroupBy_Sum_With_Result_Selector
		{
			get { return ""; }
		}

		public string GroupBy_With_Anon_Element
		{
			get { return ""; }
		}

		public string GroupBy_With_Element_Selector
		{
			get { return ""; }
		}

		public string GroupBy_With_Element_Selector_Sum
		{
			get { return ""; }
		}

		public string GroupBy_With_OrderBy
		{
			get { return ""; }
		}

		public string Join_To_Categories
		{
			get { return ""; }
		}

		public string OrderBy_CustomerID
		{
			get { return ""; }
		}

		public string OrderBy_CustomerID_Descending
		{
			get { return ""; }
		}

		public string OrderBy_CustomerID_Descending_ThenBy_City
		{
			get { return ""; }
		}

		public string OrderBy_CustomerID_Descending_ThenByDescending_City
		{
			get { return ""; }
		}

		public string OrderBy_CustomerID_OrderBy_Company_City
		{
			get { return ""; }
		}

		public string OrderBy_CustomerID_ThenBy_City
		{
			get { return ""; }
		}

		public string OrderBy_CustomerID_With_Select
		{
            get
            {
                return @"SELECT t0.Address
FROM Customers t0
ORDER BY t0.CustomerID";
            }
		}

		public string OrderBy_Join
		{
			get { return ""; }
		}

		public string OrderBy_SelectMany
		{
			get { return ""; }
		}

		public string Paging_With_Skip_Take
		{
			get { return ""; }
		}

		public string Paging_With_Take
		{
			get { return ""; }
		}

		public string Select_0_When_Set_False
		{
			get { return ""; }
		}

		public string Select_100_When_Set_True
		{
			get { return ""; }
		}

		public string Select_Anon_Constant_Int
		{
			get { return ""; }
		}

		public string Select_Anon_Constant_NullString
		{
			get { return ""; }
		}

		public string Select_Anon_Empty
		{
			get { return ""; }
		}

		public string Select_Anon_Literal
		{
			get { return ""; }
		}

		public string Select_Anon_Nested
		{
			get { return ""; }
		}

		public string Select_Anon_One
		{
            get { return @"SELECT t0.ProductName 
                             FROM Products"; }
		}

		public string Select_Anon_One_And_Object
		{
			get { return ""; }
		}

		public string Select_Anon_Three
		{
			get { return ""; }
		}

		public string Select_Anon_Two
		{
			get { return ""; }
		}

		public string Select_Anon_With_Local
		{
			get { return ""; }
		}

		public string Select_Nested_Collection
		{
			get { return ""; }
		}

		public string Select_Nested_Collection_With_AnonType
		{
			get { return ""; }
		}

		public string Select_On_Self
		{
			get { return ""; }
		}

		public string Select_Scalar
		{
			get { return ""; }
		}

		public string SelectMany_Customer_Orders
		{
			get { return ""; }
		}

		public string Where_Resolves_String_EndsWith_Literal
		{
			get { return ""; }
		}

		public string Where_Resolves_String_EndsWith_OtherColumn
		{
			get { return ""; }
		}

		public string Where_Resolves_String_IsNullOrEmpty
		{
			get { return ""; }
		}

		public string Where_Resolves_String_Length
		{
			get { return ""; }
		}

		public string Where_Resolves_String_StartsWith_Literal
		{
			get { return ""; }
		}

		public string Where_Resolves_String_StartsWith_OtherColumn
		{
			get { return ""; }
		}
	}
}
