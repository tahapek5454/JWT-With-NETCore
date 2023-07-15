using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs
{
    public class ErrorDto
    {
        public List<string> Errors { get; }
        public bool IsShow { get; set; }

        public ErrorDto()
        {
            Errors= new List<string>();
        }
        public ErrorDto(string error, bool isShow) : this()
        {
            Errors.Add(error);
            IsShow = isShow;
        }

        public ErrorDto(List<string> errors, bool ısShow)
        {
            Errors = errors;
            IsShow = ısShow;
        }

        
    }
}
