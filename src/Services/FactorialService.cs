using System.Threading.Tasks;

public class FactorialService
{
    public async Task<double> Calculate(int number){
        return await Task.Factory.StartNew(() =>{
            double result = 1;
            while(number != 1){
                result = result * number;
                number = number - 1;
            }
            return result;
        });
    }

    public async Task<double> Sleep(int number){
        return await Task.Factory.StartNew(() =>{
            System.Threading.Thread.Sleep(number * 1000);
            return number;
        });
    }
}