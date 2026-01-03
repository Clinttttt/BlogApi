using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request.Newsletter
{
    public record SendNewsletterRequest(
        string PostTitle,
        string PostContent,  
        string PostSlug);
   
}
