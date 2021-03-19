using Amazon.CDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EksSimple
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new EksSimpleStack(app, "EksSimpleStack");
            app.Synth();
        }
    }
}
