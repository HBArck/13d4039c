﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostBinary.Classes
{

    class NumberUtil
    {
        /// <summary>
        /// Struct stores Float and Integer part of Number
        /// </summary>
        public struct IFPartsOfNumber
        {
            public String IntegerPart;
            public String FloatPart;
        }
        private ProgramCore PCoreInst = null;
        public NumberUtil(ProgramCore newPCoreInst)
        {
            if (PCoreInst == null)
            {
                PCoreInst = newPCoreInst;
            }
        }
       
        /// <summary>
        /// Function converts from scientific notation to normal notation.
        /// Number must be in scientific notation.
        /// </summary>
        /// <param name="str">Number in scientific notation. (1,23e+4)</param>
        /// <returns>Number in normal notation. (12300)</returns>
        public String ScientificToNormal(String str)
        {
            return str.Replace("e", "*10^(") + ")";
        }

        /// <summary>
        /// Function counts significant characters in current number 
        /// Number must be in scientific notation.
        /// </summary>
        /// <param name="str">Number in which significant characters should be found. (1,123e-56)</param>
        /// <returns>Counted number of significant characters in number. (4)</returns>
        public int CountSignificantCharacters(String str)
        {
            return str.Split('e')[0].Remove(str.Split('e')[0].IndexOf(','), 1).Trim('-').Length;
        }

        /// <summary>
        /// Function finds exponent of the number.
        /// Number must be in scientific notation.
        /// </summary>
        /// <param name="str">Number in which exponent should be found. (1,123e-56)</param>
        /// <returns>Number exponent. (56)</returns>
        public String NumberExponent(String str)
        {
            return str.Split('e')[str.Split('e').Count() - 1].Trim('-');
        }

        /// <summary>
        /// Function converts from normal notation to scientific notation.
        /// </summary>
        /// <param name="str">Number in normal notation. (12300)</param>
        /// <returns>Number in scientific notation. (1,23e+4)</returns>
        public String NormalToScientific(String str)
        {
            return "";
        }


        public String NormalizeNumber(String dataString, int inAccuracy, NumberFormat inNumberFormat)
        {
            /// Current Number Sign 0 = '+'; 1 = '-'
            String Sign;
            try
            {
                if (dataString.Length > inAccuracy)
                    dataString = dataString.Substring(0, inAccuracy);

                if (dataString.Contains("E"))
                    dataString = dataString.Replace('E', 'e');
                //else
                // return null || Rise FCCoreException
                if (dataString.Contains("."))
                    dataString = dataString.Replace('.', ',');
                //else
                // return null || Rise FCCoreException

                if (dataString.IndexOf(',') == 0)
                    dataString = "0" + dataString;


                if ((dataString[0] != '-') && (dataString[0] != '+'))
                {
                    dataString = "+" + dataString;
                    if (inNumberFormat == 0)
                    {
                        Sign = "0";
                        //SignLeft = "0";
                    }
                    else
                    {
                        /** Reserved for float and interval formats
                        if (Left_Right == PartOfNumber.Left)
                            SignLeft = "0";
                        else
                            SignRight = "0";
                         */
                    }
                }
                else
                {
                    if (inNumberFormat == 0)
                    {
                        Sign = "0";
                        //SignLeft = "1";
                    }
                    else
                    {
                        /** Reserved for float and interval formats
                        if (Left_Right == PartOfNumber.Left)
                            SignLeft = "1";
                        else
                            SignRight = "1";
                         */
                    }
                }


                if (dataString.IndexOf(',') == -1)
                    if (dataString.IndexOf('e') != -1)
                        dataString = dataString.Substring(0, dataString.IndexOf('e')) + ",0" + dataString.Substring(dataString.IndexOf('e'));
                    else
                        dataString = dataString + ",0";

                if ((dataString[dataString.IndexOf('e') + 1] != '+') &&
                    (dataString[dataString.IndexOf('e') + 1] != '-'))
                    dataString = dataString.Replace("e", "e+");

                if (dataString.IndexOf('e') == -1)
                    dataString = dataString + "e+0";
            }
            catch (Exception ex)
            {
                throw new Exception("NormalizeNumber:" + ex.Message);
            }
            return dataString;
        }

        /// <summary>
        /// Denormolizes number
        /// </summary>
        /// <param name="dataString">Input String to dernomolize</param>
        /// <param name="NumberFormat">Indicates format of input number</param>
        /// <returns>Denormolized number as String</returns>
        public IFPartsOfNumber DenormalizeNumber(String dataString, NumberFormat inNumberFormat)
        {
            String denormNumber = "";
            String denormIntPart = "", denormFloatPart = "";
            String ExpSign, Sign, SignCharacter="+";
            String E;
            String[] tempArray;
            IFPartsOfNumber returnValue = new IFPartsOfNumber();
            try
            {
                ExpSign = dataString.Substring(dataString.IndexOf('e') + 1, 1);
                if (dataString[0] == '+')
                {
                    if (inNumberFormat == 0)
                    {
                        Sign = "0";
                        SignCharacter = "+";
                    }
                    else
                    {
                        /*
                        if (Left_Right == PartOfNumber.Left)
                        {
                            SignLeft = "0";
                            SignCharacterLeft = "+";
                        }
                        else
                        {
                            SignRight = "0";
                            SignCharacterRight = "+";
                        }*/
                    }
                }
                else
                    if (dataString[0] == '-')
                    {
                        if (inNumberFormat == 0)
                        {
                            Sign = "1";
                            SignCharacter = "-";
                        }
                        else
                        {
                            /*
                            if (Left_Right == PartOfNumber.Left)
                            {
                                SignLeft = "1";
                                SignCharacterLeft = "-";
                            }
                            else
                            {
                                SignRight = "1";
                                SignCharacterRight = "-";
                            }*/
                        }
                    }
                    else
                    {
                        if (inNumberFormat == 0)
                        {
                            Sign = "0";
                            SignCharacter = "+";
                        }
                        else
                        {
                            /*
                            if (Left_Right == PartOfNumber.Left)
                            {
                                SignLeft = "0";
                                SignCharacterLeft = "+";
                            }
                            else
                            {
                                SignRight = "0";
                                SignCharacterRight = "+";
                            }*/
                        }
                    }
                //throw new Exception("Func [selectSEM]:= NoSignException.");

                int index = dataString.IndexOf('e') + 1;
                if (index < dataString.Length)
                    E = dataString.Substring(index, dataString.Length - index);
                else
                    throw new Exception("Func [selectSEM]:= NoExponentaException.");

                int iExp = Math.Abs(int.Parse(E));
                if ((dataString[0] == '-') || (dataString[0] == '+'))
                    dataString = dataString.Substring(1);
                /*iPart */
                denormIntPart = dataString.Substring(0, dataString.IndexOf(','));
                index = dataString.IndexOf(',') + 1;

                /*fPart*/
                denormFloatPart = dataString.Substring(index, dataString.IndexOf('e') - index);////+1
                if (ExpSign == "+")
                {
                    String fPartTemp = denormFloatPart;
                    if (iExp > 0)
                    {
                        tempArray = new String[Math.Abs(iExp - denormFloatPart.Length)];
                        for (int i = 0; i < (Math.Abs(iExp - denormFloatPart.Length)); i++)
                            tempArray[i] = "0";
                        fPartTemp = fPartTemp + String.Join("", tempArray);
                        denormFloatPart = "0";
                    }
                    denormIntPart = denormIntPart + fPartTemp.Substring(0, iExp);
                    denormFloatPart = fPartTemp.Substring(iExp);
                    if (denormFloatPart.Length == 0)
                        denormFloatPart = "0";
                }
                else
                {
                    String iPartTemp = denormIntPart;
                    tempArray = new String[Math.Abs(iExp - denormIntPart.Length)];
                    for (int i = 0; i < Math.Abs((iExp - denormIntPart.Length)); i++)
                        tempArray[i] = "0";
                    iPartTemp = String.Join("", tempArray) + iPartTemp;
                    if (iExp > denormIntPart.Length)
                    {
                        denormFloatPart = iPartTemp + denormFloatPart;
                        denormIntPart = "0";
                    }
                    else
                    {
                        denormFloatPart = iPartTemp.Substring(iPartTemp.Length - iExp) + denormFloatPart;
                        if (iPartTemp.Length != iExp)
                            denormIntPart = iPartTemp.Substring(0, iPartTemp.Length - iExp);
                        else
                            denormIntPart = "0";
                    }
                }
                // iPart = myUtil.deleteZeroFromNumber(iPart);
                // if (iPart[0] == '0')
                //    iPart = iPart.Substring(1);
                while ((denormIntPart[0] == '0') && (denormIntPart.Length > 1))
                {
                    denormIntPart = denormIntPart.Substring(1);
                }
                // Compact to one statement num32 = num64 = num128 = num256 = denorm
                
                denormNumber = SignCharacter + denormIntPart + "," + denormFloatPart;
                
                //Num32.Denormalized = Num64.Denormalized = Num128.Denormalized = Num256.Denormalized = 
                if (inNumberFormat == 0)
                {
                    /*Num32.IntPartDenormalized = Num64.IntPartDenormalized = Num128.IntPartDenormalized = Num256.IntPartDenormalized = IntPartDenormalized = denormIntPart;
                    Num32.FloatPartDenormalized = Num64.FloatPartDenormalized = Num128.FloatPartDenormalized = Num256.FloatPartDenormalized = FloatPartDenormalized = denormFloatPart;*/
                    //DenormIntPart = denormIntPart;
                    returnValue.IntegerPart = denormIntPart;
                    //DenormFloatPart = denormFloatPart;
                    returnValue.FloatPart = denormFloatPart;
                }
                else
                {
                    /*
                    if (Left_Right == PartOfNumber.Left)
                    {
                        IntPartDenormalizedFILeft = denormIntPart;
                        FloatPartDenormalizedFILeft = denormFloatPart;
                    }
                    else
                    {
                        IntPartDenormalizedFIRight = denormIntPart;
                        FloatPartDenormalizedFIRight = denormFloatPart;
                    }
                    //Num32.IntPartDenormalizedFI = Num64.IntPartDenormalizedFI = Num128.IntPartDenormalizedFI = Num256.IntPartDenormalizedFI = IntPartDenormalizedFI = denormIntPart;
                    //Num32.FloatPartDenormalizedFI = Num64.FloatPartDenormalizedFI = Num128.FloatPartDenormalizedFI = Num256.FloatPartDenormalizedFI = FloatPartDenormalizedFI = denormFloatPart;
                    DenormIntPartFI = denormIntPart;
                    DenormFloatPartFI = denormFloatPart;
                    */
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                //throw new FCCoreFunctionException("Exception in Func ['selectSEM'] Mess=[" + ex.Message + "]");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
        public const int ACCURANCY = 2000;
        public static int AdditionalAccurancy = 0;
        /// <summary>
        /// Converts float part of number form 10cc to 2cc.Lenght depends from constant 'ACCURANCY'
        /// Funcs using: NONE
        /// Vars using : ACCURANCY
        /// </summary>
        /// <param name="inString">Input Number to convert</param>
        /// <returns>Returns number float part in 2cc</returns>
        public static String convert10to2FPart(String inString)
        {// accurancy   -> ACCURANCY
            String result = "";
            String outString = "";
            int plusOne = 0;
            int countAccuracy, i, currNumber;

            try
            {
                if (inString == "0")
                {
                    result = "0";
                    return result;
                }

                for (countAccuracy = 0; countAccuracy < ACCURANCY + AdditionalAccurancy; countAccuracy++)
                {

                    outString = "";
                    plusOne = 0;
                    for (i = inString.Length; i > 0; i--)
                    {
                        currNumber = int.Parse(inString[i - 1].ToString());
                        if (currNumber < 5)
                        {
                            outString = (currNumber * 2 + plusOne).ToString() + outString;
                            plusOne = 0;
                        }
                        else
                        {
                            outString = (currNumber * 2 + plusOne - 10).ToString() + outString;
                            plusOne = 1;
                            if (i == 1)
                                outString = "1" + outString;
                        }
                    }

                    if (countAccuracy != ACCURANCY)
                    {
                        if (outString.Length > inString.Length)
                        {
                            result = result + "1";
                            outString = outString.Substring(1);
                        }
                        else
                        {
                            result = result + "0";
                        }

                    }
                    inString = outString;
                }
            }
            catch (Exception ex)
            {
                //throw new FCCoreException();
                throw new Exception("Func [convert10to2FPart]:=" + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Converts integer part of number form 10cc to 2cc.Lenght depends only from number
        /// Funcs using: NONE
        /// Vars using : NONE
        /// </summary>
        /// <param name="inString">Input Number to convert</param>
        /// <returns>Returns number integer part in 2cc</returns>
        public static String convert10to2IPart(String inString)
        {
            String result = ""; 	// Результат каждого деления
            String balanse = ""; 	// Массив остатков от каждого деления (исходное число в 2 с/с)
            String balanseTemp = "";// Временная переменная для подсчета остатка
            int activeDividend;		// Текущее делимое
            bool saveMinus = false;
            int i = 0;

            try
            {
                if (inString.IndexOf(',') != -1)
                    inString = inString.Substring(0, inString.IndexOf(','));
                if (inString[0] == '-')
                {
                    saveMinus = true;
                    i++;
                }
                else
                    if (inString[0] == '+')
                        inString = inString.Substring(1);

                while ((inString[i] == '0') && (inString.Length > 1))
                {
                    if (!saveMinus)
                        inString = inString.Substring(1);
                    else
                        inString = "-" + inString.Substring(2);
                }

                result = inString;
                int iRes;
                while (true)
                {	          // цикл по всем делениям (14,7,3,1)
                    /*
                     *        14 |_2_
                     *        14 |7	 |_2_
                     *		  --  6  |3  |_2_
                     *		   0  --  2  |1  
                     *			  1   --
                     *				  1	
                     *
                     *									balanse=1110
                     *					result=14
                     *					result=7
                     *					   ...
                     *					result=1
                     */
                    if (result == "")
                        break;

                    inString = result;
                    result = "";
                    inString = inString + ("0");
                    activeDividend = int.Parse(inString[0].ToString());

                    for (i = 0; i < (inString.Length - 1); i++)
                    { // деление предыдущего результата  
                        switch (activeDividend)
                        {
                            case 0:
                                {
                                    result = result + ("0");
                                    break;
                                }
                            case 1:
                                {
                                    if (i != 0)
                                        result = result + ("0");
                                    if (i == (inString.Length - 2))
                                        balanseTemp = "1";
                                    break;
                                }
                            default:
                                {
                                    iRes = activeDividend / 2;
                                    result = result + ((iRes));
                                    activeDividend %= 2;
                                    balanseTemp = activeDividend.ToString();
                                    break;
                                }
                        }
                        if ((activeDividend != 0) || (inString[i + 1] != '0'))
                            activeDividend = int.Parse((activeDividend).ToString() + inString[i + 1].ToString());
                    }
                    balanse = balanseTemp + (balanse);

                    if (result.Length == 1)//  Выход из цикла 
                    {
                        int iTemp = int.Parse(result);	//  когда результат=1, или =0
                        if ((iTemp == 0) || (iTemp == 1))				//
                            break;								//
                    }
                }
                balanse = result + (balanse);
            }
            catch (Exception ex)
            {
                //throw new FCCoreException();
                throw new Exception("Func [convert10to2IPart]:=" + ex.Message);
            }
            return balanse;
        }

    }
}
