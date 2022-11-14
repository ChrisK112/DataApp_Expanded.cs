using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace TbManagementTool
{
    class DataTableFactory
    {
        public static DataTable DtCgSpec()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Primkey", typeof(String));
            dt.Columns.Add("PersonRef", typeof(String));
            dt.Columns.Add("ClientName", typeof(String));
            dt.Columns.Add("AddedBy", typeof(String));
            dt.Columns.Add("AddedDateTime", typeof(String));
            dt.Columns.Add("Title", typeof(String));
            dt.Columns.Add("FirstName", typeof(String));
            dt.Columns.Add("MiddleName", typeof(String));
            dt.Columns.Add("Surname", typeof(String));
            dt.Columns.Add("Salutation", typeof(String));
            dt.Columns.Add("AddressLine1", typeof(String));
            dt.Columns.Add("AddressLine2", typeof(String));
            dt.Columns.Add("AddressLine3", typeof(String));
            dt.Columns.Add("TownCity", typeof(String));
            dt.Columns.Add("County", typeof(String));
            dt.Columns.Add("Postcode", typeof(String));
            dt.Columns.Add("Country", typeof(String));
            dt.Columns.Add("OrganisationName", typeof(String));
            dt.Columns.Add("TelephoneNumber", typeof(String));
            dt.Columns.Add("MobileNumber", typeof(String));
            dt.Columns.Add("EmailAddress", typeof(String));
            dt.Columns.Add("AppealCode", typeof(String));
            dt.Columns.Add("PackageCode", typeof(String));
            dt.Columns.Add("Deceased", typeof(String));
            dt.Columns.Add("Goneaway", typeof(String));
            dt.Columns.Add("NoFurtherCommunication", typeof(String));
            dt.Columns.Add("PreloadedCAFNumber", typeof(String));
            dt.Columns.Add("ColdURN", typeof(String));
            dt.Columns.Add("ImportFile", typeof(String));
            dt.Columns.Add("RaffleStartNumber", typeof(String));
            dt.Columns.Add("RaffleEndNumber", typeof(String));
            dt.Columns.Add("RecordType", typeof(String));
            dt.Columns.Add("GiftAid", typeof(String));
            dt.Columns.Add("Campaign", typeof(String));
            dt.Columns.Add("PhonePreference", typeof(String));
            dt.Columns.Add("MailPreference", typeof(String));
            dt.Columns.Add("EmailPreference", typeof(String));
            dt.Columns.Add("SMSPreference", typeof(String));
            dt.Columns.Add("ThirdPartyPreference", typeof(String));
            dt.Columns.Add("Barcode", typeof(String));
            dt.Columns.Add("ClientData1", typeof(String));
            dt.Columns.Add("ClientData2", typeof(String));
            dt.Columns.Add("ClientData3", typeof(String));
            dt.Columns.Add("ClientData4", typeof(String));
            dt.Columns.Add("ClientData5", typeof(String));
            dt.Columns.Add("ClientData6", typeof(String));
            dt.Columns.Add("ClientData7", typeof(String));
            dt.Columns.Add("ClientData8", typeof(String));
            dt.Columns.Add("ClientData9", typeof(String));
            dt.Columns.Add("ClientData10", typeof(String));
            dt.Columns.Add("MembershipStartDate", typeof(String));
            dt.Columns.Add("MembershipEndDate", typeof(String));
            dt.Columns.Add("MembershipStatus", typeof(String));
            dt.Columns.Add("MembershipType", typeof(String));

            return dt;
        }

        public static DataTable DtMiScanSpec()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("URN", typeof(String));
            dt.Columns.Add("Cold URN", typeof(String));
            dt.Columns.Add("ClientName", typeof(String));
            dt.Columns.Add("Title", typeof(String));
            dt.Columns.Add("FirstName/Initial", typeof(String));
            dt.Columns.Add("MiddleName", typeof(String));
            dt.Columns.Add("Surname", typeof(String));
            dt.Columns.Add("Salutation", typeof(String));
            dt.Columns.Add("OrganisationName", typeof(String));
            dt.Columns.Add("Address1", typeof(String));
            dt.Columns.Add("Address2", typeof(String));
            dt.Columns.Add("Address3", typeof(String));
            dt.Columns.Add("TownCity", typeof(String));
            dt.Columns.Add("County", typeof(String));
            dt.Columns.Add("Postcode", typeof(String));
            dt.Columns.Add("Country", typeof(String));
            dt.Columns.Add("TelephoneNumber", typeof(String));
            dt.Columns.Add("MobileNumber", typeof(String));
            dt.Columns.Add("EmailAddress", typeof(String));
            dt.Columns.Add("WorkEmailAddress", typeof(String));
            dt.Columns.Add("AppealCode", typeof(String));
            dt.Columns.Add("PackageCode", typeof(String));
            dt.Columns.Add("Campaign", typeof(String));
            dt.Columns.Add("GiftAid", typeof(String));
            dt.Columns.Add("NoFurtherCommunication", typeof(String));
            dt.Columns.Add("PhonePreference", typeof(String));
            dt.Columns.Add("MailPreference", typeof(String));
            dt.Columns.Add("EmailPreference", typeof(String));
            dt.Columns.Add("SMSPreference", typeof(String));
            dt.Columns.Add("ThirdPartyPreference", typeof(String));
            dt.Columns.Add("Spare1", typeof(String));
            dt.Columns.Add("Spare2", typeof(String));
            dt.Columns.Add("Spare3", typeof(String));
            dt.Columns.Add("Spare4", typeof(String));
            dt.Columns.Add("Spare5", typeof(String));
            dt.Columns.Add("Spare6", typeof(String));
            dt.Columns.Add("Spare7", typeof(String));
            dt.Columns.Add("Spare8", typeof(String));
            dt.Columns.Add("Spare9", typeof(String));
            dt.Columns.Add("Spare10", typeof(String));
            dt.Columns.Add("Spare11", typeof(String));
            dt.Columns.Add("Spare12", typeof(String));
            dt.Columns.Add("Spare13", typeof(String));
            dt.Columns.Add("Spare14", typeof(String));
            dt.Columns.Add("Spare15", typeof(String));
            dt.Columns.Add("Spare16", typeof(String));
            dt.Columns.Add("Spare17", typeof(String));
            dt.Columns.Add("Spare18", typeof(String));
            dt.Columns.Add("Spare19", typeof(String));
            dt.Columns.Add("Spare20", typeof(String));
            dt.Columns.Add("Spare21", typeof(String));
            dt.Columns.Add("Spare22", typeof(String));
            dt.Columns.Add("Spare23", typeof(String));
            dt.Columns.Add("Spare24", typeof(String));
            dt.Columns.Add("Spare25", typeof(String));

            return dt;
        }
    }
}
