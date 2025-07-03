using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Configs;
public class BankApiConfig : IEntityTypeConfiguration<BankApi>
{
    public void Configure(EntityTypeBuilder<BankApi> builder)
    {
        builder.ToTable("bankapi");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.Method).HasColumnName("method").HasMaxLength(0).IsRequired();
        builder.Property(e => e.StatusCode).HasColumnName("status_code").HasMaxLength(0).IsRequired();
        builder.Property(e => e.Ip).HasColumnName("ip").HasMaxLength(0).IsRequired();
        builder.Property(e => e.Url).HasColumnName("url").HasMaxLength(0).IsRequired();
        builder.Property(e => e.Header).HasColumnName("header").HasMaxLength(0).IsRequired();
        builder.Property(e => e.Payload).HasColumnName("payload").HasMaxLength(0).IsRequired();
        builder.Property(e => e.RequestDate).HasColumnName("request_date").HasColumnType("timestamp").IsRequired();
        builder.Property(e => e.ResponseDate).HasColumnName("response_date").HasColumnType("timestamp");
        builder.Property(e => e.Elapse).HasColumnName("elapse").IsRequired();
        builder.Property(e => e.Response).HasColumnName("response");
        builder.Property(e => e.BankId).HasColumnName("bank_id").IsRequired();
        builder.Property(e => e.BankRef).HasColumnName("bank_ref");
        builder.Property(e => e.BillerCode).HasColumnName("biller_code");
        builder.Property(e => e.BillCode).HasColumnName("bill_code");
        builder.Property(e => e.CustomerCode).HasColumnName("customer_code");
        builder.Property(e => e.TranId).HasColumnName("tran_id");
        builder.Property(e => e.TranDate).HasColumnName("tran_date").HasColumnType("timestamp");
        builder.Property(e => e.Code).HasColumnName("code");
        builder.Property(e => e.Message).HasColumnName("message");
        builder.Property(e => e.Operation).HasColumnName("operation");
    }
}
