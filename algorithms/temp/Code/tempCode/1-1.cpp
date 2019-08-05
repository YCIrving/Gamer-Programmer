#include <iostream>
#include <math.h>

using namespace std;

double splitNums[]={5000, 3000, 9000, 13000, 10000, 20000, 25000};
double splitRate[] = {0, 0.03, 0.1, 0.2, 0.25, 0.3, 0.35};

double calculate(double n)
{
    double ans=0;
    for(int i=0; i<7; i++)
    {
        if(n - splitNums[i]<=0) return (ans + (double)n*splitRate[i]);
        else
        {
            n -= splitNums[i];
            ans += (splitNums[i] * splitRate[i]);
        }
        //cout<<ans<<"haha"<<endl;
    }
    return (ans+ n*0.45);
}
int main()
{
    int t;
    cin>>t;
    while(t)
    {
        double n;
        cin>>n;
        cout<<round(calculate(n))<<endl;
        //printf("%.0f\n", calculate(n));
        //cout<< calculate(n) <<endl;
        t--;
    }
    return 0;
}
