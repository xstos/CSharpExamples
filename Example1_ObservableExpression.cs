using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Subjects;
using System.Reactive.Linq;
namespace CSharpExamples
{
    public class ObservableExpression<T> : IObservable<T>
    {
        Func<T> getValue;
        Subject<T> subject = new Subject<T>();

        public Func<T> Getter => getValue;

        public ObservableExpression<T> Set(T value)
        {
            this.getValue = () => value;
            subject.OnNext(value);
            return this;
        }

        public static implicit operator ObservableExpression<T>(T value)
        {
            return new ObservableExpression<T>() { getValue = ()=>value };
        }

        public static implicit operator Func<T>(ObservableExpression<T> oe)
        {
            return oe;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return subject.Subscribe(observer);
        }

        static Func<T> plus(dynamic first,ObservableExpression<T> second)
        {
            return () => first + second.getValue();
        }

        public static ObservableExpression<T> operator +(ObservableExpression<T> oe1, ObservableExpression<T> oe2)
        {
            var ret = new ObservableExpression<T>();
            oe1.Distinct().Subscribe((i) =>
            {
                ret.getValue = plus(i, oe2);
                ret.OnNext();
            });
            oe2.Distinct().Subscribe((i) =>
            {
                ret.getValue = plus(i, oe1);
                ret.OnNext();
            });

            ret.getValue = () => (dynamic)oe1.getValue() + (dynamic)oe2.getValue();
            ret.OnNext();
            return ret;
        }
        public static ObservableExpression<T> operator -(ObservableExpression<T> oe1, ObservableExpression<T> oe2)
        {
            //todo: add more operators and methods...
            throw new NotImplementedException();
        }

        private void OnNext()
        {
            this.subject.OnNext(this.getValue());
        }
    }

    public static partial class Examples
    {
        public static void Example1()
        {
            Console.WriteLine("Example1");
            ObservableExpression<int> subExpr = 0;

            var myExpr = subExpr + 1000 + subExpr;

            myExpr.Distinct().Subscribe(Console.WriteLine);

            subExpr.Set(1);   //prints 1002
            subExpr.Set(123); //prints 1246
        }
    }
}
