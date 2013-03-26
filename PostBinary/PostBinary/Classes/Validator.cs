﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PostBinary.Classes
{

    class Validator
    {
        public String inputStr = "";


        public Validator()
        { }


        public Responce.validationResponce validate(String inStr)
        {
            Responce.validationResponce response = new Responce.validationResponce();

            response = vCharacters(inStr);
            if (!response.error)
            {
                response = vSequenceOfNumVar(inStr);
                if (!response.error)
                {
                    response = vBreckets(inStr);
                }
            }
            return response;

        }
        /*
         * validate number of brackets
         */
        private Responce.validationResponce vBreckets(String inStr)
        {
            bool error = false;
            int errorBegin = -1;
            int errorEnd = -1;
            Responce.validationResponce response = new Responce.validationResponce();

            int openedBrackets = 0;
            int closedBrackets = 0;

            CharEnumerator ce = inStr.GetEnumerator();
            while (ce.MoveNext())
            {
                if ('(' == ce.Current) { openedBrackets++; }
                else if (')' == ce.Current) { closedBrackets++; }
            }
            if (openedBrackets != closedBrackets)
            {
                error = true;
                errorBegin = inStr.LastIndexOf("(");
                errorEnd = inStr.LastIndexOf(")");
            }
            response.setValidationResponce(error, errorBegin, errorEnd, new ValidatorErrorType());
            return response;
        }
        private Responce.validationResponce vCharacters(String inStr)
        {
            bool error = false;
            int errorBegin = -1;
            int errorEnd = -1;
            Responce.validationResponce response = new Responce.validationResponce();
            Regex rgx = new Regex(@"[*+^\-/a-zA-Z\d\(\)]+");
            MatchCollection mc = rgx.Matches(inStr);
            if (mc.Count == 0)
            {
                error = true;
                errorBegin = 0;
                errorEnd = 0;
            }
            response.setValidationResponce(error, errorBegin, errorEnd, new ValidatorErrorType());
            return response;
        }
        /*
         * validate sequence of numbers and variables 
         */
        private Responce.validationResponce vSequenceOfNumVar(String inStr)
        {
            bool error = false;
            int errorBegin = -1;
            int errorEnd = -1;
            Responce.validationResponce response = new Responce.validationResponce();

            Regex rgx = new Regex(@"[*+^\-/]{2}");

            MatchCollection mc = rgx.Matches(inStr);

            if (mc.Count > 0)
            {
                error = true;
                errorBegin = inStr.IndexOf(mc[0].ToString());
                String lastError = mc[mc.Count - 1].ToString();
                errorEnd = inStr.LastIndexOf(lastError) + lastError.Length - 1;
            }
            response.setValidationResponce(error, errorBegin, errorEnd, new ValidatorErrorType());
            return response;
        }
    }
}
