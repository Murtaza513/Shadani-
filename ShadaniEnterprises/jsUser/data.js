/**
 * Data and settings for the company
 *
 * How to properly enter your data:
 * Be sure when entering your information to enclose that data with double quotes. When entering numbers they
 * don't need to be enclosed with quotes, in general the sample data below should be used as general guide on
 * how to properly enter your data. And if you have double (or single) quotes in your data (like My "Awesome" Company)
 * than you should use something we call escaping special characters with the backslash sign (\) so the final
 * company name will be "My \"Awesome\" Company".
 */

var ib_invoice_data = {
  "{company_name}"          : "Shadani Enterprises",
  "{company_address}"       : "Flat No. A-307, Farhan Tower Block-20",
  "{company_city_zip_state}": " ● Gulistan-e-Johar, Karachi",
  "{company_phone_fax}"     : "Phone: 0213-34156849",
    "{company_email_web}": "www.shadanienterprises.com ● jp@shadanienterprises.com",
  "{payment_info1}"         : "PAYMENTS",
  "{payment_info2}"         : "Account Number ● 123006705",
  "{payment_info3}"         : "IBAN ● US100000060345",
  "{payment_info4}"         : "SWIFT ● BOA447",
  "{payment_info5}"         : "",
  "{issue_date_label}"      : "Issued on - ",
  "{issue_date}"            : "",
  "{net_term_label}"        : "Net - ",
  "{net_term}"              : 21,
  "{due_date_label}"        : "Due on - ",
  "{due_date}"              : "",
  "{currency_label}"        : "* All prices are in ",
  "{currency}"              : "USD",
  "{po_number_label}"       : "Ref # - ",
  "{po_number}"             : "1/3-147",
  "{bill_to_label}"         : "Client",
  "{client_name}"           : "Slate Rock and Gravel Company",
  "{client_address}"        : "222 Rocky Way",
  "{client_city_zip_state}" : "30000 Bedrock, Cobblestone County",
  "{client_phone_fax}"      : "+555 7 123-5555",
  "{client_email}"          : "fred@slaterockgravel.bed",
  "{client_other}"          : "Attn: Fred Flintstone",
  "{invoice_title}"         : "INVOICE",
  "{invoice_number}"        : "#001",
  "{item_row_number_label}": "",
  
    "{item_description_label}": "Tender Code",
    "{item_quantity_label}": "Product Registration Number",
    "{item_price_label}": "Name Of Products",
  "{item_discount_label}"   : "Discount",
    "{item_tax_label}": "Pack Size",
    "{item_line_total_label}": "A/c Unit",
    "{item_tax_label2}": "Quantity",
    "{item_line_total_label2}": "Batch/Lot",
    "{item_tax_label2}": "MFG",
    "{item_line_total_label2}": "Expiry",
  "{item_row_number}"       : "",
  "{item_description}"      : "",
  "{item_quantity}"         : "",
  "{item_price}"            : "",
  "{item_discount}"         : "",
  "{item_tax}"              : "",
  "{item_line_total}"       : "",
  "{amount_subtotal_label}" : "Subtotal",
  "{amount_subtotal}"       : "",
  "{tax_name}"              : "Tax",
  "{tax_value}"             : "",
  "{amount_total_label}"    : "Total",
  "{amount_total}"          : "",
  "{amount_paid_label}"     : "Paid",
  "{amount_due_label}"      : "Due sum",
  "{amount_due}"            : "",
  "{terms_label}"           : "Terms",
  "{terms}"                 : "Fred, thank you very much. We really appreciate your business.<br />Please send payments before the due date.",

  // Settings
  "date_format"             : "mm/dd/yyyy", // One of dd/mm/yyyy, dd-mm-yyyy, mm/dd/yyyy, mm-dd-yyyy
  "currency_position"       : "left", // One of left or right
  "default_columns"         : ["item_row_number", "item_description", "item_quantity", "item_price", "item_discount", "item_tax", "item_line_total"],
  "default_quantity"        : "1",
  "default_price"           : "0",
  "default_discount"        : "0",
  "default_tax"             : "0",
  "default_number_rows"     : 3,
  "auto_calculate_dates"    : true,
  "load_items"              : true,
  "invoicebus_fineprint"    : true,

  // Items
  "items": [
    {
      "item_description" : "Frozen Brontosaurus Ribs",
      "item_quantity"    : "2",
      "item_price"       : "120",
      "item_discount"    : "",
      "item_tax"         : "2%"
    },
    {
      "item_description" : "Mammoth Chops",
      "item_quantity"    : "14",
      "item_price"       : "175",
      "item_discount"    : "10%",
      "item_tax"         : "5%"
    },
    {
      "item_description" : "",
      "item_quantity"    : "",
      "item_price"       : "",
      "item_discount"    : "",
      "item_tax"         : ""
    }
  ]
};
