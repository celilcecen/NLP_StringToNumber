using NLP_StringToNumber.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace NLP_StringToNumber.Controllers
{
    public class StrToNumberController : ApiController
    {
        // POST api/values
        public string Post([FromBody] StrToNumber_RequestDto oModel)
        {
            string strInput = oModel.UserText;
            string strOutput = "";

            StrToNumberDto strToNumberDto = new StrToNumberDto() { StrDirty = strInput };

            if (strInput != null && strInput != "")
            {
                string[] strInputArray = strInput.Split(' ');

                foreach (string strInputSubString in strInputArray)
                {
                    StrToNumberDto subStrToNumberDto = ParseStringToNumber(strInputSubString);

                    if (!subStrToNumberDto.IsNumber)
                    {
                        subStrToNumberDto.SubStrToNumberDtoList = FindSubStrToNumberDtos(subStrToNumberDto.StrClean);
                    }

                    strToNumberDto.SubStrToNumberDtoList.Add(subStrToNumberDto);
                }
            }

            List<int> tempNumericList = new List<int>();
            foreach (StrToNumberDto subStrToNumberDto in strToNumberDto.SubStrToNumberDtoList)
            {
                if (subStrToNumberDto.IsNumber)
                {
                    tempNumericList.Add(subStrToNumberDto.Number);
                }
                else
                {
                    if (subStrToNumberDto.SubStrToNumberDtoList.Count > 0)
                    {
                        foreach (StrToNumberDto subStrToNumberDtoInner in subStrToNumberDto.SubStrToNumberDtoList)
                        {
                            if (subStrToNumberDtoInner.IsNumber)
                            {
                                tempNumericList.Add(subStrToNumberDtoInner.Number);
                            }
                            else
                            {
                                if (tempNumericList.Count > 0)
                                {
                                    strOutput += strOutput == "" ? "" : " ";
                                    strOutput += GetCleanNumberByNumericList(tempNumericList).ToString();
                                    tempNumericList = new List<int>();
                                }


                                strOutput += strOutput == "" ? subStrToNumberDtoInner.StrClean : "" + subStrToNumberDtoInner.StrClean;

                            }
                        }
                    }
                    else
                    {
                        if (tempNumericList.Count > 0)
                        {
                            strOutput += strOutput == "" ? "" : " ";
                            strOutput += GetCleanNumberByNumericList(tempNumericList).ToString();
                            tempNumericList = new List<int>();
                        }

                        strOutput += strOutput == "" ? subStrToNumberDto.StrClean : " " + subStrToNumberDto.StrClean;
                    }
                }
            }


            if (tempNumericList.Count > 0 && String.IsNullOrEmpty(strOutput))
            {
                strOutput += strOutput == "" ? "" : " ";
                strOutput += GetCleanNumberByNumericList(tempNumericList).ToString();
            }

            strToNumberDto.StrClean = strOutput;

            return strOutput;
        }

        #region Utility Methods

        public static StrToNumberDto ParseStringToNumber(string strInput)
        {
            StrToNumberDto strToNumberDto = new StrToNumberDto() { StrDirty = strInput, StrClean = strInput };

            strToNumberDto.Number = 0;
            strToNumberDto.IsNumber = false;

            if (strInput != null && strInput != "")
            {
                strToNumberDto.IsNumber = true;
                switch (strInput.ToLower())
                {
                    case "one": strToNumberDto.Number = 1; break;
                    case "two": strToNumberDto.Number = 2; break;
                    case "three": strToNumberDto.Number = 3; break;
                    case "four": strToNumberDto.Number = 4; break;
                    case "five": strToNumberDto.Number = 5; break;
                    case "six": strToNumberDto.Number = 6; break;
                    case "seven": strToNumberDto.Number = 7; break;
                    case "eight": strToNumberDto.Number = 8; break;
                    case "nine": strToNumberDto.Number = 9; break;
                    case "ten": strToNumberDto.Number = 10; break;
                    case "teen": strToNumberDto.Number = 10; break;
                    case "eleven": strToNumberDto.Number = 11; break;
                    case "twelve": strToNumberDto.Number = 12; break;                    
                    case "thirteen": strToNumberDto.Number = 13; break;
                    case "fourteen": strToNumberDto.Number = 14; break;
                    case "fifteen": strToNumberDto.Number = 15; break;
                    case "sixteen": strToNumberDto.Number = 16; break;
                    case "seventeen": strToNumberDto.Number = 17; break;
                    case "eighteen": strToNumberDto.Number = 18; break;
                    case "nineteen": strToNumberDto.Number = 19; break;
                    case "twenty": strToNumberDto.Number = 20; break;
                    case "thirty": strToNumberDto.Number = 30; break;
                    case "fourty": strToNumberDto.Number = 40; break;
                    case "fifty": strToNumberDto.Number = 50; break;
                    case "sixty": strToNumberDto.Number = 60; break;
                    case "seventy": strToNumberDto.Number = 70; break;
                    case "eighty": strToNumberDto.Number = 80; break;
                    case "ninety": strToNumberDto.Number = 90; break;
                    case "hundred": strToNumberDto.Number = 100; break;
                    case "thousand": strToNumberDto.Number = 1000; break;
                    case "million": strToNumberDto.Number = 1000000; break;
                    case "billion": strToNumberDto.Number = 1000000000; break;
                    default: strToNumberDto.Number = 0; strToNumberDto.IsNumber = false; break;
                }
            }

            return strToNumberDto;
        }

        public static List<StrToNumberDto> FindSubStrToNumberDtos(string strInput)
        {
            List<StrToNumberDto> strToNumberDtoList = new List<StrToNumberDto>();

            if (strInput != null && strInput != "")
            {
                string[] strPaternArray = new string[] {
                        "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
                        "ten","teen","eleven","twelve", "twenty", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen","thirty", "fourty", "fifty", "sixty", "seventy", "eighty", "ninety",
                        "hundred", "thousand", "million", "billion"
                    };

                List<KeyValuePair<int, string>> tempKeyValuePairList = new List<KeyValuePair<int, string>>();

                foreach (string strPatern in strPaternArray)
                {
                    foreach (Match match in Regex.Matches(strInput, @"" + strPatern))
                    {
                        tempKeyValuePairList.Add(new KeyValuePair<int, string>(match.Index, match.Value));
                    }
                }

                string last = "";
                if (tempKeyValuePairList.Count > 0)
                {

                    tempKeyValuePairList = tempKeyValuePairList.OrderBy(x => x.Key).ToList();

                    List<int> tempNumericList = new List<int>();

                    foreach (KeyValuePair<int, string> tempKeyValuePair in tempKeyValuePairList)
                    {
                        int tempNumeric = ParseStringToNumber(tempKeyValuePair.Value).Number;

                        strToNumberDtoList.Add(new StrToNumberDto { StrClean = tempKeyValuePair.Value, IsNumber = true, Number = tempNumeric });

                        tempNumericList.Add(tempNumeric);
                    }

                    last = strInput.Substring(strInput.IndexOf(tempKeyValuePairList.Last().Value) + tempKeyValuePairList.Last().Value.Length);

                    if (last != "") { strToNumberDtoList.Add(new StrToNumberDto { StrDirty = strInput, StrClean = last, IsNumber = false, Number = 0 }); }

                }
            }

            return strToNumberDtoList;
        }

        public static int GetCleanNumberByNumericList(List<int> nNumericList)
        {
            int nCleanNumber = 0;

            int nDigitNumber_K = 0;
            int nDigitNumber_M = 0;
            int nDigitNumber_B = 0;


            foreach (int nNumeric in nNumericList)
            {
                if (nCleanNumber > nNumeric)
                {
                    nCleanNumber += nNumeric;
                }
                else
                {
                    if (nCleanNumber == 0) { nCleanNumber = 1; }

                    nCleanNumber *= nNumeric;
                }

                if (nNumeric == 1000) { nDigitNumber_K = nCleanNumber; nCleanNumber = 0; }
                if (nNumeric == 1000000) { nDigitNumber_M = nCleanNumber; nCleanNumber = 0; }
                if (nNumeric == 1000000000) { nDigitNumber_B = nCleanNumber; nCleanNumber = 0; }
            }

            return nCleanNumber + nDigitNumber_K + nDigitNumber_M + nDigitNumber_B;
        }

        #endregion
    }
}
