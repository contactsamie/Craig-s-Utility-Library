﻿/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

using Utilities.DataTypes.ExtensionMethods;
using System.Data;

namespace UnitTests.DataTypes.ExtensionMethods
{
    public class ObjectExtensions
    {
        [Fact]
        public void IsTest()
        {
            object TestObject = null;
            Assert.True(TestObject.Is(x => x == null));
            TestObject = 1;
            Assert.False(TestObject.Is(x => x == null));
            int TestObject2 = 1;
            Assert.False(TestObject2.Is(x => x < 2, x => x > 3));
            Assert.True(TestObject2.Is(x => x < 2, x => x > 0));
            Assert.True(TestObject2.Is(1));
            Assert.False(TestObject2.Is(2));
        }

        [Fact]
        public void Check()
        {
            object TestObject = null;
            Assert.Equal(3, TestObject.Check(x => x != null, 3));
            Assert.Equal(null, TestObject.Check(x => x == null, 3));
            Assert.Equal(3, TestObject.Check(3));
            TestObject = 3;
            Assert.Equal(3, TestObject.Check(2));
        }


        [Fact]
        public void Times()
        {
            Assert.Equal(new int[] { 0, 1, 2, 3, 4 }.ToList(), 5.Times(x => x));
            StringBuilder Builder = new StringBuilder();
            5.Times(x => { Builder.Append(x); });
            Assert.Equal("01234", Builder.ToString());
        }

        [Fact]
        public void Chain()
        {
            DateTime Temp = new DateTime(1999, 1, 1);
            Assert.Equal(Temp, Temp.Chain<DateTime>(x => x.AddSeconds(1)));
        
            Temp = new DateTime(1999, 1, 1);
            Assert.Equal(Temp.AddSeconds(1), Temp.Chain(x => x.AddSeconds(1)));
        
            Temp = new DateTime(1999, 1, 1);
            Assert.Equal(Temp, Temp.Chain<DateTime>(x => x.AddSeconds(1)));
            Assert.Equal(default(DateTime?), ((DateTime?)null).Chain<DateTime?>(x => x.Value.AddSeconds(1)));
            Assert.Throws<ArgumentOutOfRangeException>(() => ((DateTime?)null).Chain<DateTime?>(x => x.Value.AddSeconds(1), DateTime.MaxValue));
        
            Temp = new DateTime(1999, 1, 1);
            Assert.Equal(Temp.AddSeconds(1), Temp.Chain(x => x.AddSeconds(1)));
            Assert.Equal(DateTime.MaxValue, ((DateTime?)null).Chain(x => x.Value.AddSeconds(1), DateTime.MaxValue));
        
            Assert.Null(new MyTestClass().Chain(x => x.A));
            Assert.NotNull(new MyTestClass().Chain(x => x.A, new MyTestClass()));
            Assert.Equal(10, new MyTestClass().Chain(x => x.A, new MyTestClass()).Chain(x => x.B));
            Assert.Equal(0, new MyTestClass().Chain(x => x.A).Chain(x => x.B));
            Assert.Equal(0, ((MyTestClass)null).Chain(x => x.A).Chain(x => x.B));
        }


        private class MyTestClass : IMyTestClass
        {
            public MyTestClass() { B = 10; }
            public virtual MyTestClass A { get; set; }
            public virtual int B { get; set; }
        }

        private interface IMyTestClass
        {
        }

    }
}