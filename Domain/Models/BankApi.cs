using System;

namespace Domain.Models
{
    public class BankApi
    {
        public int Id { get; set; }
        public string? Method { get; set; }
        public string? StatusCode { get; set; }
        public string? Ip { get; set; }
        public string? Url { get; set; }
        public string? Header { get; set; }
        public string? Payload { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public int? Elapse { get; set; }
        public string? Response { get; set; }
        public string? BankId { get; set; }
        public string? BankRef { get; set; }
        public string? BillerCode { get; set; }
        public string? BillCode { get; set; }
        public string? CustomerCode { get; set; }
        public string? TranId { get; set; }
        public DateTime? TranDate { get; set; }
        public string? Code { get; set; }
        public string? Message { get; set; }
        public string? Operation { get; set; }
    }
}
