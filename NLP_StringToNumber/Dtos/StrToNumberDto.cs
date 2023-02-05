using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NLP_StringToNumber.Dtos
{
    public class StrToNumberDto
    {
        public string StrDirty { get; set; }
        public string StrClean { get; set; }
        public bool IsNumber { get; set; }
        public int Number { get; set; }
        public List<StrToNumberDto> SubStrToNumberDtoList { get; set; } = new List<StrToNumberDto>();
    }
}