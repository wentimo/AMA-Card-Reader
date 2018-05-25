using System.Windows;

namespace AMA_Card_Reader.Models
{
	public class AMACardEntry : Notifiable
	{
		public string Name => $"{Firstname}, {Lastname}";

		private int rowNumber;
		public int RowNumber
		{
			get { return rowNumber; }
			set { SetField(ref rowNumber, value); }
		}

		private string firstName;
		public string Firstname
		{
			get { return firstName; }
			set { SetField(ref firstName, value); }
		}

		private string lastName;
		public string Lastname
		{
			get { return lastName; }
			set { SetField(ref lastName, value); }
		}

		private string address;
		public string Address
		{
			get { return address; }
			set { SetField(ref address, value); }
		}

		private string city;
		public string City
		{
			get { return city; }
			set { SetField(ref city, value); }
		}

		private string state;
		public string State
		{
			get { return state; }
			set { SetField(ref state, value); }
		}

		private string zipCode;
		public string Zipcode
		{
			get { return zipCode; }
			set { SetField(ref zipCode, value); }
		}

		private string amaCardNumber;
		public string AMACardNumber
		{
			get { return amaCardNumber; }
			set { SetField(ref amaCardNumber, value); }
		}

		private string expiration;
		public string Expiration
		{
			get { return expiration; }
			set { SetField(ref expiration, value); }
		}

		private string ridetype;
		public string RideType
		{
			get { return ridetype; }
			set { SetField(ref ridetype, value); }
		}
		private string ridernumber;
		public string RiderNumber
		{
			get { return ridernumber; }
			set { SetField(ref ridernumber, value); }
		}
		private string phone;
		public string Phone
		{
			get { return phone; }
			set { SetField(ref phone, value); }
		}
		private string email;
		public string Email
		{
			get { return email; }
			set { SetField(ref email, value); }
		}
		private string make;
		public string Make
		{
			get { return make; }
			set { SetField(ref make, value); }
		}
		private string model;
		public string Model
		{
			get { return model; }
			set { SetField(ref model, value); }
		}
		private string cc;
		public string CC
		{
			get { return cc; }
			set { SetField(ref cc, value); }
		}
		private string year;
		public string Year
		{
			get { return year; }
			set { SetField(ref year, value); }
		}
		private string day1;
		public string Day1
		{
			get { return day1; }
			set { SetField(ref day1, value); }
		}
		private string day2;
		public string Day2
		{
			get { return day2; }
			set { SetField(ref day2, value); }
		}
		private string barbecue;
		public string Barbecue
		{
			get { return barbecue; }
			set { SetField(ref barbecue, value); }
		}
		private string camping;
		public string Camping
		{
			get { return camping; }
			set { SetField(ref camping, value); }
		}
		private string tshirt;
		public string TShirt
		{
			get { return tshirt; }
			set { SetField(ref tshirt, value); }
		}
		private string paidamount;
		public string PaidAmount
		{
			get { return paidamount; }
			set { SetField(ref paidamount, value); }
		}
		private string mannerofpayment;
		public string MannerOfPayment
		{
			get { return mannerofpayment; }
			set { SetField(ref mannerofpayment, value); }
		}
		private string amapaidamount;
		public string AMAPaidAmount
		{
			get { return amapaidamount; }
			set { SetField(ref amapaidamount, value); }
		}

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
			return new AMACardEntry()
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
		}

		public string ToCSVRow(bool nextLine) => $"{(nextLine ? "\r" : "")}{Firstname},{Lastname},{Address},{City},{State},{Zipcode},{AMACardNumber},{Expiration},{RideType},{RiderNumber},{Phone},{Email},{Make},{Model},{CC},{Year},{Day1},{Day2},{Barbecue},{Camping},{TShirt},{PaidAmount},{MannerOfPayment},{AMAPaidAmount}";
	}
}
