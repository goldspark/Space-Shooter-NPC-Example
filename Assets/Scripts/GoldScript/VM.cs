using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GoldScript
{

    public enum CodeResult
    {
        Success,
        Failure
    }

    public class VM
    {

        private static VM instance;

        private int ip = 0;


        private VM() 
        {

        }

        public CodeResult ReadCode(byte[] code)
        {

            for(; ip < code.Length;)
            {

            }

            return CodeResult.Failure;
        }

    }
}
