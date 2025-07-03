using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.OutfaceModels
{
    public class BankApiRequestModel
    {
        [JsonPropertyName("method")]
        public string? Method { get; set; }

        [JsonPropertyName("status_code")]
        public string? StatusCode { get; set; }

        [JsonPropertyName("ip")]
        public string? Ip { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("header")]
        public string? Header { get; set; }

        [JsonPropertyName("payload")]
        public string? Payload { get; set; }

        [JsonPropertyName("request_date")]
        public DateTime? RequestDate { get; set; }

        [JsonPropertyName("response_date")]
        public DateTime? ResponseDate { get; set; }

        [JsonPropertyName("elapse")]
        public int? Elapse { get; set; }

        [JsonPropertyName("response")]
        public string? Response { get; set; }

        [JsonPropertyName("bank_id")]
        public string? BankId { get; set; }

        [JsonPropertyName("bank_ref")]
        public string? BankRef { get; set; }

        [JsonPropertyName("biller_code")]
        public string? BillerCode { get; set; }

        [JsonPropertyName("bill_code")]
        public string? BillCode { get; set; }

        [JsonPropertyName("customer_code")]
        public string? CustomerCode { get; set; }

        [JsonPropertyName("tran_id")]
        public string? TranId { get; set; }

        [JsonPropertyName("tran_date")]
        public DateTime? TranDate { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("operation")]
        public string? Operation { get; set; }
    }
}
