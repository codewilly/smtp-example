using System;

namespace Domain.EmailTemplates
{
    /** Important:
     * The class name must be same as html file. In this case, BasicTemplate.html
     */
    public class BasicTemplate : BaseEmailTemplate
    {
        /** Important:
         * The properties name must be same as inside the html [tags]. For example "[Subject]"
         */
        public string Subject { get; set; }

        public string Message { get; set; }

        public DateTime SentAt { get; set; }
    }
}
