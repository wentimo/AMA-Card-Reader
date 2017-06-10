using System.Windows;

namespace AMA_Card_Reader.Models
{
    public class AMACardEntry
    {
        public string Name => $"{Firstname}, {Lastname}";

        public int RowNumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string AMACardNumber { get; set; }
        public string Expiration { get; set; }
        public string RideType { get; set; }
        public string RiderNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string CC { get; set; }
        public string Year { get; set; }
        public string Day1 { get; set; }
        public string Day2 { get; set; }
        public string Barbecue { get; set; }
        public string Camping { get; set; }
        public string TShirt { get; set; }
        public string PaidAmount { get; set; }
        public string MannerOfPayment { get; set; }
        public string AMAPaidAmount { get; set; }

        public static AMACardEntry ParseEntryFromCardData(string cardData, int rowNumber)
        {
            if (cardData.Length >= 101)
            {
                int offset = 0;
                int amaIndex = cardData.IndexOf(';');
                if (amaIndex > 79)
                {
                    offset = (amaIndex - 79) + 1;
                }
                var entry = new AMACardEntry()
                {
                    RowNumber = rowNumber,
                    Firstname = cardData.Substring(1, 13).Trim(),
                    Lastname = cardData.Substring(13, 15).Trim(),
                    Address = cardData.Substring(28, 25).Trim(),
                    City = cardData.Substring(53, 15).Trim(),
                    State = cardData.Substring(69, 2).Trim(),
                    Zipcode = cardData.Substring(71, 5).Trim(),                    
                    AMACardNumber = cardData.Substring(80 + offset, 7).Trim(),
                    Expiration = $"{cardData.Substring(99 + offset, 2)}/{cardData.Substring(95 + offset, 4)}"
                };

                if (entry.Expiration == "99/9999") entry.Expiration = "LIFE";

                return entry;
            }
            else
            {
                MessageBox.Show("There was a problem trying to parse your card data. Please try reading again.");
                return null;
            }
        }

        public static AMACardEntry ParseEntryFromCSVRow(string csvRow, int rowNumber)
        {
            var data = csvRow.Split(',');

            int index = 0;
            var entry = new AMACardEntry()
            {
                RowNumber = rowNumber,
                Firstname = data[index++],
                Lastname=data[index++], 
                Address=data[index++],
                City=data[index++],
                State=data[index++],
                Zipcode = data[index++],
                AMACardNumber=data[index++],
                Expiration=data[index++],
                RideType = data[index++],
                RiderNumber = data[index++],
                Phone =data[index++],
                Email = data[index++],
                Make = data[index++],
                Model = data[index++],
                CC = data[index++],
                Year = data[index++],
                Day1 = data[index++],
                Day2 = data[index++],
                Barbecue = data[index++],
                Camping=data[index++],
                TShirt = data[index++],
                PaidAmount = data[index++],
                MannerOfPayment = data[index++],
                AMAPaidAmount = data[index++]
            };

            return entry;
        }

        public string ToCSVRow(bool nextLine) => $"{(nextLine ? "\r" : "")}{Firstname},{Lastname},{Address},{City},{State},{Zipcode},{AMACardNumber},{Expiration},{RideType},{RiderNumber},{Phone},{Email},{Make},{Model},{CC},{Year},{Day1},{Day2},{Barbecue},{Camping},{TShirt},{PaidAmount},{MannerOfPayment},{AMAPaidAmount}";
    }
}
