using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    public class CodeMethod
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public const string One = "日编+流水";
        public const string Two = "月编+流水";
        public const string Three = "流水";
        public const string Four = "手动设置";

        public static List<CodeMethod> GetCodeMethod()
        {
            List<CodeMethod> list = new List<CodeMethod>();
            list.Add(new CodeMethod()
            {
                Id = CodeMethod.One,
                Description = CodeMethod.One
            });
            list.Add(new CodeMethod(){
                Id = CodeMethod.Two,
                Description = CodeMethod.Two
            });
            list.Add(new CodeMethod(){
                Id = CodeMethod.Three,
                Description = CodeMethod.Three
            });
            list.Add(new CodeMethod(){
                Id = CodeMethod.Four,
                Description = CodeMethod.Four
            });
            return list;
        }
    }
}