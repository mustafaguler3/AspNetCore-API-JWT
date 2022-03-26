using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udemy.Shared.Dtos
{
    public class ErrorDto
    {
        public List<String> Errors { get;private set; }//dışardan başkası set etmesin sadece bu sınıftan edilsin

        public bool IsShow { get;private set; }

        public ErrorDto()
        {
            Errors = new List<string>();
        }

        public ErrorDto(string error,bool isShow)
        {
            Errors.Add(error);
            isShow = true;
        }

        public ErrorDto(List<string> errors,bool isShow)
        {
            Errors = errors;
            IsShow = isShow;
        }
    }
}
