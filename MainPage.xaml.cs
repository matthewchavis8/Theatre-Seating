using System.Collections.Specialized;
using System.Reflection.Metadata;

namespace Theatre
{
    public class SeatingUnit
    {
        public string Name { get; set; }
        public bool Reserved { get; set; }

        public SeatingUnit(string name, bool reserved = false)
        {
            Name = name;
            Reserved = reserved;
        }

    }

    public partial class MainPage : ContentPage
    {
        SeatingUnit[,] seatingChart = new SeatingUnit[5, 10];

        public MainPage()
        {
            InitializeComponent();
            GenerateSeatingNames();
            RefreshSeating();
        }

        private async void ButtonReserveSeat(object sender, EventArgs e)
        {
            var seat = await DisplayPromptAsync("Enter Seat Number", "Enter seat number: ");
            // Testing no meta data
            if (seat != null)
            {
                for (int i = 0; i < seatingChart.GetLength(0); i++)
                {
                    for (int j = 0; j < seatingChart.GetLength(1); j++)
                    {
                        if (seatingChart[i, j].Name == seat)
                        {
                            seatingChart[i, j].Reserved = true;
                            await DisplayAlert("Successfully Reserverd", "Your seat was reserverd successfully!", "Ok");
                            RefreshSeating();
                            return;
                        }
                    }
                }

                await DisplayAlert("Error", "Seat was not found.", "Ok");
            }
        }

        private void GenerateSeatingNames()
        {
            List<string> letters = new List<string>();
            for (char c = 'A'; c <= 'Z'; c++)
            {
                letters.Add(c.ToString());
            }

            int letterIndex = 0;

            for (int row = 0; row < seatingChart.GetLength(0); row++)
            {
                for (int column = 0; column < seatingChart.GetLength(1); column++)
                {
                    seatingChart[row, column] = new SeatingUnit(letters[letterIndex] + (column + 1).ToString());
                }

                letterIndex++;
            }
        }

        private void RefreshSeating()
        {
            grdSeatingView.RowDefinitions.Clear();
            grdSeatingView.ColumnDefinitions.Clear();
            grdSeatingView.Children.Clear();

            for (int row = 0; row < seatingChart.GetLength(0); row++)
            {
                var grdRow = new RowDefinition();
                grdRow.Height = 50;

                grdSeatingView.RowDefinitions.Add(grdRow);

                for (int column = 0; column < seatingChart.GetLength(1); column++)
                {
                    var grdColumn = new ColumnDefinition();
                    grdColumn.Width = 50;

                    grdSeatingView.ColumnDefinitions.Add(grdColumn);

                    var text = seatingChart[row, column].Name;

                    var seatLabel = new Label();
                    seatLabel.Text = text;
                    seatLabel.HorizontalOptions = LayoutOptions.Center;
                    seatLabel.VerticalOptions = LayoutOptions.Center;
                    seatLabel.BackgroundColor = Color.Parse("#333388");
                    seatLabel.Padding = 10;

                    if (seatingChart[row, column].Reserved == true)
                    {
                        //Change the color of this seat to represent its reserved.
                        seatLabel.BackgroundColor = Color.Parse("#883333");

                    }

                    Grid.SetRow(seatLabel, row);
                    Grid.SetColumn(seatLabel, column);
                    grdSeatingView.Children.Add(seatLabel);

                }
            }
        }

        //Assign to Matthew Chavis
        private async void ButtonReserveRange(object sender, EventArgs e) {
            var seatRangeInput = await DisplayPromptAsync("Reserve Seat Range", "Enter seat range (e.g., A1:A4):");
            if (string.IsNullOrWhiteSpace(seatRangeInput))
                return;
            

            var seats = seatRangeInput.Split(':');
            if (seats.Length != 2) {
                await DisplayAlert("Error", "Invalid format. Please use the format: A1:A4", "Ok");
                return;
            }

            string startSeat = seats[0].Trim().ToUpper();
            string endSeat = seats[1].Trim().ToUpper();

            if (startSeat.Length < 2 || endSeat.Length < 2) {
                await DisplayAlert("Error", "Invalid seat identifier.", "Ok");
                return;
            }

            char rowStart = startSeat[0];
            char rowEnd = endSeat[0];

            if (rowStart != rowEnd) {
                await DisplayAlert("Error", "Seats must be in the same row.", "Ok");
                return;
            }

            if (!int.TryParse(startSeat.Substring(1), out int startColumn) ||
                !int.TryParse(endSeat.Substring(1), out int endColumn)) {
                await DisplayAlert("Error", "Invalid seat number.", "Ok");
                return;
            }

            int rowIndex = rowStart - 'A';

            if (rowIndex < 0 || rowIndex >= seatingChart.GetLength(0)) {
                await DisplayAlert("Error", "Invalid row.", "Ok");
                return;
            }

            if (startColumn < 1 || endColumn < 1 || startColumn > seatingChart.GetLength(1) || endColumn > seatingChart.GetLength(1)) {
                await DisplayAlert("Error", "Seat number out of range.", "Ok");
                return;
            }

            if (startColumn > endColumn) {
                await DisplayAlert("Error", "Invalid range. The starting seat must be before the ending seat.", "Ok");
                return;
            }

            bool allAvailable = true;
            for (int col = startColumn - 1; col < endColumn; col++) {
                if (seatingChart[rowIndex, col].Reserved) {
                    allAvailable = false;
                    break;
                }
            }

            if (!allAvailable) {
                await DisplayAlert("Error", "One or more seats in the range are already reserved.", "Ok");
                return;
            }

            for (int col = startColumn - 1; col < endColumn; col++)
                seatingChart[rowIndex, col].Reserved = true;

            await DisplayAlert("Success", "Seats reserved successfully!", "Ok");
            RefreshSeating();
        }

        //Assigned to Danielle Daye
        private void ButtonCancelReservation(object sender, EventArgs e)
        {
             private async void ButtonCancelReservation(object sender, EventArgs e)
            {
                var seat = await DisplayPromptAsync("Enter Seat Number", "Enter seat number: ");
               
                if (seat != null)
                {
                    for (int i = 0; i < seatingChart.GetLength(0); i++)
                    {
                        for (int j = 0; j < seatingChart.GetLength(1); j++)
                        {
                        if (seatingChart[i, j].Name == seat)
                            {
                            seatingChart[i, j].Reserved = true;
                            await DisplayAlert("Successfully Cancelled", "Your seat was cancelled successfully!", "Ok");
                            RefreshSeating();
                            return;
                            }
                        }
                    }
                    await DisplayAlert("Error", "Seat was not found.", "Ok");
                }
            
            }
        }

        //Assign to Team 3 Member
        private void ButtonCancelReservationRange(object sender, EventArgs e)
        {

        }

        //Assign to Team 4 Member
        private void ButtonResetSeatingChart(object sender, EventArgs e)
        {

        }
    }

}

