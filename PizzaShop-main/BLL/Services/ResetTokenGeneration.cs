using Pizza_Shop_Management_System.Services.Interfaces;

namespace Pizza_Shop_Management_System.Services;

public class ResetTokenGeneration
{
    public string TokenGenerator()
    {
        string characters = "qwertyuioplkjhgfdsazxcvbnm1234567890QWERTYUIOPLKJHGFDSAZXCVBNM";
        Random rnd = new Random();
        string random_code = "";

        for (int i = 0; i < 6; i++)
        {
            int random_digit = rnd.Next(0,characters.Length);
            random_code += characters[random_digit];   
        }

        return random_code;
    }
}
