using System.Text;
namespace WinApp_IO
{
    public partial class FormReadWrite : Form
    {
        public FormReadWrite()
        {
            InitializeComponent();
        }
        static int max = 12;        // Maximum number of names
        static int attributes = 3;  // Name, Phone, Position
        int ptr = 0;                // Current length of array with data
        private string[,] ArrayEmpInfo = new string[max, attributes];
        //========================================//
        //        Minimal error trapping!
        //========================================//
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (ptr < max)
            {
                try
                {
                    ArrayEmpInfo[ptr, 0] = textBoxName.Text;
                    ArrayEmpInfo[ptr, 1] = textBoxPhone.Text;
                    ArrayEmpInfo[ptr, 2] = textBoxPosition.Text;
                    ptr++;
                }
                catch
                {
                    MessageBox.Show("Well that didnt work!");
                }
            }
            else
            {
                MessageBox.Show("The array is FULL");
            }
            Clear_TextBoxes();
            Sort();
            DisplayListNames();
        }
        private void Clear_TextBoxes()
        {
            textBoxName.Clear();
            textBoxPhone.Clear();
            textBoxPosition.Clear();
        }
        private void Sort()
        {
            for (int x = 1; x < max; x++)
            {
                for (int i = 0; i < max - 1; i++)
                {
                    if (!string.IsNullOrEmpty(ArrayEmpInfo[i + 1, 0]))
                    {
                        if (string.Compare(ArrayEmpInfo[i, 0], ArrayEmpInfo[i + 1, 0]) == 1)
                        {
                            for (int z = 0; z < attributes; z++)
                            {
                                string temp = ArrayEmpInfo[i, z];
                                ArrayEmpInfo[i, z] = ArrayEmpInfo[i + 1, z];
                                ArrayEmpInfo[i + 1, z] = temp;
                            }
                        }
                    }
                }
            }
        }
        private void DisplayListNames()
        {
            listBoxNames.Items.Clear();
            string rec = "";
            for (int x = 0; x < max; x++)
            {
                rec = ArrayEmpInfo[x, 0] + " \t" + ArrayEmpInfo[x, 1];
                listBoxNames.Items.Add(rec);
            }
        }
        private void listBoxNames_MouseClick(object sender, MouseEventArgs e)
        {
            if (listBoxNames.SelectedIndex != -1)
            {
                string curItem = listBoxNames.SelectedItem.ToString();
                int indx = listBoxNames.FindString(curItem);
                listBoxNames.SetSelected(indx, true);
                textBoxName.Text = ArrayEmpInfo[indx, 0];
                textBoxPhone.Text = ArrayEmpInfo[indx, 1];
                textBoxPosition.Text = ArrayEmpInfo[indx, 2];
            }
            else
            {
                MessageBox.Show("Please select a name");
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            using (var stream = File.Open("employee.dat", FileMode.Create))
            {
                using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
                {
                    for (int x = 0; x < ptr; x++)
                    {
                        for (int y = 0; y < attributes; y++)
                        {
                            writer.Write(ArrayEmpInfo[x, y]);
                        }
                    }
                }
            }
        }
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (File.Exists("employee.dat"))
            {
                using (var stream = File.Open("employee.dat", FileMode.Open))
                {
                    using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                    {
                        int x = 0;
                        Array.Clear(ArrayEmpInfo);
                        while (stream.Position < stream.Length)
                        {
                            for (int y = 0; y < attributes; y++)
                            {
                                ArrayEmpInfo[x, y] = reader.ReadString();
                            }
                            x++;
                        }
                        ptr = x;
                    }
                }
                DisplayListNames();
            }
        }
    }
}