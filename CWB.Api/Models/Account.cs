﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWB.Api.Models
{

    public class AccountsResponse
    {
        public string odatacontext { get; set; }
        public Account[] value { get; set; }
    }

    public class Account
    {
        public string odataetag { get; set; }
        public int address2_addresstypecode { get; set; }
        public bool merged { get; set; }
        public int statecode { get; set; }
        public float exchangerate { get; set; }
        public string name { get; set; }
        public int opendeals { get; set; }
        public DateTime modifiedon { get; set; }
        public string _owninguser_value { get; set; }
        public int openrevenue_state { get; set; }
        public int accountratingcode { get; set; }
        public bool marketingonly { get; set; }
        public bool donotphone { get; set; }
        public int preferredcontactmethodcode { get; set; }
        public string _ownerid_value { get; set; }
        public int customersizecode { get; set; }
        public DateTime openrevenue_date { get; set; }
        public float openrevenue_base { get; set; }
        public int businesstypecode { get; set; }
        public bool donotemail { get; set; }
        public int address2_shippingmethodcode { get; set; }
        public string address1_addressid { get; set; }
        public int address2_freighttermscode { get; set; }
        public int statuscode { get; set; }
        public DateTime createdon { get; set; }
        public int msdyn_travelchargetype { get; set; }
        public int opendeals_state { get; set; }
        public int versionnumber { get; set; }
        public bool donotpostalmail { get; set; }
        public float openrevenue { get; set; }
        public bool donotsendmm { get; set; }
        public bool donotfax { get; set; }
        public bool donotbulkpostalmail { get; set; }
        public bool creditonhold { get; set; }
        public string _transactioncurrencyid_value { get; set; }
        [System.ComponentModel.DataAnnotations.Key]
        public string accountid { get; set; }
        public bool donotbulkemail { get; set; }
        public string _modifiedby_value { get; set; }
        public bool followemail { get; set; }
        public int shippingmethodcode { get; set; }
        public string _createdby_value { get; set; }
        public int territorycode { get; set; }
        public bool msdyn_taxexempt { get; set; }
        public bool participatesinworkflow { get; set; }
        public int accountclassificationcode { get; set; }
        public string _owningbusinessunit_value { get; set; }
        public string address2_addressid { get; set; }
        public DateTime opendeals_date { get; set; }
        public object lastusedincampaign { get; set; }
        public object address1_name { get; set; }
        public object address1_telephone2 { get; set; }
        public object overriddencreatedon { get; set; }
        public object adx_createdbyipaddress { get; set; }
        public object entityimageid { get; set; }
        public object ownershipcode { get; set; }
        public object address2_line1 { get; set; }
        public object msdyn_travelcharge { get; set; }
        public object _primarycontactid_value { get; set; }
        public object creditlimit { get; set; }
        public object entityimage_url { get; set; }
        public object _msdyn_preferredresource_value { get; set; }
        public object timezoneruleversionnumber { get; set; }
        public object entityimage_timestamp { get; set; }
        public object telephone3 { get; set; }
        public object address1_freighttermscode { get; set; }
        public object _preferredsystemuserid_value { get; set; }
        public object _msdyn_salestaxcode_value { get; set; }
        public object adx_createdbyusername { get; set; }
        public object onholdtime { get; set; }
        public object telephone2 { get; set; }
        public object address2_primarycontactname { get; set; }
        public object utcconversiontimezonecode { get; set; }
        public object address1_fax { get; set; }
        public object address1_composite { get; set; }
        public object _createdonbehalfby_value { get; set; }
        public object address2_city { get; set; }
        public object address2_latitude { get; set; }
        public object entityimage { get; set; }
        public object _originatingleadid_value { get; set; }
        public object marketcap { get; set; }
        public object aging90_base { get; set; }
        public object address2_postalcode { get; set; }
        public object address2_name { get; set; }
        public object primarysatoriid { get; set; }
        public object _masterid_value { get; set; }
        public object aging30 { get; set; }
        public object address2_county { get; set; }
        public object emailaddress3 { get; set; }
        public object accountcategorycode { get; set; }
        public object websiteurl { get; set; }
        public object revenue { get; set; }
        public object address1_shippingmethodcode { get; set; }
        public object address2_country { get; set; }
        public object description { get; set; }
        public object paymenttermscode { get; set; }
        public object aging30_base { get; set; }
        public object address1_stateorprovince { get; set; }
        public object lastonholdtime { get; set; }
        public object _msdyn_workhourtemplate_value { get; set; }
        public object sic { get; set; }
        public object _msa_managingpartnerid_value { get; set; }
        public object creditlimit_base { get; set; }
        public object address2_telephone1 { get; set; }
        public object address2_longitude { get; set; }
        public object _defaultpricelevelid_value { get; set; }
        public object address1_primarycontactname { get; set; }
        public object address1_latitude { get; set; }
        public object address1_county { get; set; }
        public object address2_postofficebox { get; set; }
        public object _msdyn_serviceterritory_value { get; set; }
        public object _preferredserviceid_value { get; set; }
        public object address1_upszone { get; set; }
        public object stageid { get; set; }
        public object address2_composite { get; set; }
        public object adx_modifiedbyipaddress { get; set; }
        public object aging60 { get; set; }
        public object _slainvokedid_value { get; set; }
        public object customertypecode { get; set; }
        public object telephone1 { get; set; }
        public object address1_postofficebox { get; set; }
        public object address1_line2 { get; set; }
        public object msdyn_taxexemptnumber { get; set; }
        public object address1_postalcode { get; set; }
        public object traversedpath { get; set; }
        public object numberofemployees { get; set; }
        public object tickersymbol { get; set; }
        public object address2_upszone { get; set; }
        public object _msdyn_billingaccount_value { get; set; }
        public object address1_city { get; set; }
        public object aging90 { get; set; }
        public object address1_longitude { get; set; }
        public object revenue_base { get; set; }
        public object xxc_accounttype { get; set; }
        public object address1_telephone1 { get; set; }
        public object _owningteam_value { get; set; }
        public object address2_line2 { get; set; }
        public object msdyn_travelcharge_base { get; set; }
        public object primarytwitterid { get; set; }
        public object timespentbymeonemailandmeetings { get; set; }
        public object _modifiedbyexternalparty_value { get; set; }
        public object accountnumber { get; set; }
        public object address1_line1 { get; set; }
        public object teamsfollowed { get; set; }
        public object _slaid_value { get; set; }
        public object ftpsiteurl { get; set; }
        public object _preferredequipmentid_value { get; set; }
        public object yominame { get; set; }
        public object processid { get; set; }
        public object address2_telephone2 { get; set; }
        public object address1_addresstypecode { get; set; }
        public object address1_utcoffset { get; set; }
        public object _parentaccountid_value { get; set; }
        public object _createdbyexternalparty_value { get; set; }
        public object address2_fax { get; set; }
        public object aging60_base { get; set; }
        public object _modifiedonbehalfby_value { get; set; }
        public object stockexchange { get; set; }
        public object preferredappointmentdaycode { get; set; }
        public object address1_line3 { get; set; }
        public object fax { get; set; }
        public object address2_telephone3 { get; set; }
        public object sharesoutstanding { get; set; }
        public object adx_modifiedbyusername { get; set; }
        public object _territoryid_value { get; set; }
        public object address1_country { get; set; }
        public object address2_line3 { get; set; }
        public object msdyn_workorderinstructions { get; set; }
        public object address2_utcoffset { get; set; }
        public object emailaddress1 { get; set; }
        public object address2_stateorprovince { get; set; }
        public object preferredappointmenttimecode { get; set; }
        public object emailaddress2 { get; set; }
        public object marketcap_base { get; set; }
        public object importsequencenumber { get; set; }
        public object industrycode { get; set; }
        public object address1_telephone3 { get; set; }
        public override int GetHashCode()
        {
            int hash = 1;
            var propertyCollection = typeof(Account).GetProperties();
            foreach(var pi in propertyCollection)
            {
                object value = null;
                try
                {
                    value = pi.GetValue(this);
                    if(value != null)
                    {
                        hash += value.GetHashCode();
                    }
                }
                catch
                { }
            }
            return hash;
        }
    }

}