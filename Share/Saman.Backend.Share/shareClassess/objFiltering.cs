using System.Linq.Expressions;
using System.Reflection;

namespace Saman.Backend.Share.shareClasses
{
    public class objFiltering
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? SearchValue { get; set; }
        public List<Condition>? Conditions { get; set; }
        public List<Order>? Orders { get; set; }

        public void AddCondition(string key, object value)
        {
            if (this.Conditions == null) { this.Conditions = new List<Condition>(); }

            else { this.RemoveCondition(key); }

            this.Conditions.Add(new Condition(key, value));

        }
        public void RemoveCondition(string key)
        {
            if (this.Conditions != null && !string.IsNullOrWhiteSpace(key))         
            {
                this.Conditions = this.Conditions.Where(x => x.Key != key).ToList();
            }
        }

        public void AddOrder(string key, OrderType orderType)
        {
            if (this.Orders == null) { this.Orders = new List<Order>(); }

            else { this.RemoveOrder(key); }

            this.Orders.Add(new Order(key, orderType));

        }
        public void RemoveOrder(string key)
        {
            if (this.Orders != null && !string.IsNullOrWhiteSpace(key))         
            {
                this.Orders = this.Orders.Where(x => x.Key != key).ToList();
            }
        }
    }

    public class Condition
    {
        public Condition() { }
        public Condition(string key, object value) { Key = key; Value = value; }
        public Condition(string key, object value, OperationType operation) { Key = key; Value = value; Operation = operation; }

        public string Key { get; set; } = null!;
        public object Value { get; set; } = null!;
        public OperationType Operation { get; set; } = OperationType.Equal;

        public Func<T, bool> getExpression<T>()
        {
            Expression condition;
            var entityType = Expression.Parameter(typeof(T));
            var property = Expression.PropertyOrField(entityType, this.Key);
            if (property.Type == typeof(string)) { this.Value = shareConvertor.strFarsi(this.Value); }
            dynamic constant =
                (property.Type.IsGenericType || property.Type.IsEnum)
                ? Expression.Convert(Expression.Constant(this.Value), property.Type)
                : Expression.Constant(Convert.ChangeType(this.Value, property.Type));

            try
            {
                switch (this.Operation)
                {
                    case OperationType.Equal:
                        condition = Expression.Equal(property, constant);
                        break;

                    case OperationType.NotEqual:
                        condition = Expression.NotEqual(property, constant);
                        break;

                    case OperationType.Contain:
                        var notNullCheck = Expression.NotEqual(property, Expression.Constant(null, typeof(string)));
                        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        var containsExpression = Expression.Call(property, containsMethod, constant);
                        condition = Expression.AndAlso(notNullCheck, containsExpression);
                        break;

                    case OperationType.StartWith:
                        var notNullCheckStartWith = Expression.NotEqual(property, Expression.Constant(null, typeof(string)));
                        MethodInfo mi = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) })!;
                        var startWithExpression = Expression.Call(property, mi, constant);
                        condition = Expression.AndAlso(notNullCheckStartWith, startWithExpression);
                        break;

                    case OperationType.GreaterThan:
                        condition = Expression.GreaterThan(property, constant);
                        break;

                    case OperationType.GreaterThanOrEqual:
                        condition = Expression.GreaterThanOrEqual(property, constant);
                        break;

                    case OperationType.LessThan:
                        condition = Expression.LessThan(property, constant);
                        break;

                    case OperationType.LessThanOrEqual:
                        condition = Expression.LessThanOrEqual(property, constant);
                        break;

                    default:
                        condition = Expression.Equal(property, constant);
                        break;
                }
            }
            catch
            {
                condition = Expression.Equal(property, constant);
            }

            return Expression.Lambda<Func<T, bool>>(condition, entityType).Compile();
        }
    }

    public class Order
    {
        public Order() { }
        public Order(string key) { Key = key; }
        public Order(string key, OrderType orderType) { Key = key; OrderType = orderType; }

        public string Key { get; set; } = null!;
        public OrderType OrderType { get; set; } = OrderType.Ascending;

        public IEnumerable<T> getOrdering<T>(IEnumerable<T> query)
        {
            var type = typeof(T);
            var sortProperty = type.GetProperty(this.Key);
            return (this.OrderType == OrderType.Ascending) ? query.OrderBy(p => sortProperty?.GetValue(p, null)) : query.OrderByDescending(p => sortProperty?.GetValue(p, null));
        }
    }

    public enum OperationType : short
    {
        Equal = 1,
        NotEqual = 2,
        GreaterThan = 3,
        GreaterThanOrEqual = 4,
        LessThan = 5,
        LessThanOrEqual = 6,
        StartWith = 9,
        Contain = 10
    }

    public enum OrderType : short
    {
        Ascending = 0,
        Descending = 1
    }

}
