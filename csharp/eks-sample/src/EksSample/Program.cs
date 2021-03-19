using Amazon.CDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EksSample
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new EksSampleStack(app, "EksSampleStack");
            app.Synth();
        }
    }
}
