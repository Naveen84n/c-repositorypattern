namespace Lkq.Core.RulesRepo.Common
{
    /// <summary>
    /// Constants - Maintain all the constants
    /// </summary>
    public class Constants
    {
        public const int OK = 200;
        public const int CREATED = 201;
        public const int NOTFOUND = 404;
        public const int BADREQUEST = 400;
        public const int SERVERERROR = 500;

        public const string SUCCESS = "Success";
        public const string FAILED = "Failed";
        public const string STATUSMESSAGE_CREATED = "Successfully Created";
        public const string STATUSMESSAGE_SERVER_ERROR = "Something went wrong. Please try again later.";
        public const string STATUSMESSAGE_INVALID_REQUEST = "Invalid request. Please check the input value and data type.";

        //Configuration settings
        public const string COMPNINE_URL = "http://pvsinternal/api";

        public const string MESSAGE_INVALID_ATTRIBUTENAME = "Please enter valid Attribute Name.";
        public const string MESSAGE_ATTRIBUTEINFO_NOTAVAILABLE = "Attribute Name not available. Please enter valid Attribute Name.";

        public const string MESSAGE_RULE_ALREADYEXISTS = "Rule already exists with the given parameters.";
        public const string VinPosition = "Position ";
        public const string EngineVINTableName = "EngineVIN";
        public const string VinPositionApplicationNote = "ApplicationNote6";
        public const string MESSAGE_INVALID_RULEID = "Please enter valid Rule ID";
        
        public const string MESSAGE_RULE_NOTAVAILABLE = "Rule not available for the given Rule ID";
        public const string MESSAGE_RULES_NOTAVAILABLE = "Rules not available for the given input";
        public const string MESSAGE_RULE_NOTFOUND = "Rule not found for the given Rule ID";
        public const string MESSAGE_RULE_DELETED = "Rule deleted successfully";

        public const string MESSAGE_INVALID_ATTRIBUTEID = "Please enter valid Attribute ID";
        public const string MESSAGE_INVALID_ATTRIBUTELOOKUP = "Please enter valid Attribute Lookup value";
        public const string MESSAGE_INVALID_PARTTYPE = "Please enter valid Part Type";
        public const string MESSAGE_INVALID_DESCRIPTION = "Please enter valid Rule Description";
        public const string MESSAGE_INVALID_DATASOURCEID = "Please enter valid DataSource ID";
        public const string MESSAGE_INVALID_PROPERTYPATH = "Please enter valid Property Path";
        public const string MESSAGE_INVALID_PROPERTYVALUE = "Please enter valid Property Value";
        public const string MESSAGE_INVALID_ORDINAL = "Please enter valid Ordinal";
        public const string MESSAGE_INVALID_USER = "Please enter valid User Name";
        public const string MESSAGE_INVALID_PARTRULEID = "Please enter valid Rule ID";

        public const string MESSAGE_INVALID_PARTTYPELIST = "Please enter valid Part Types";


    }
}
