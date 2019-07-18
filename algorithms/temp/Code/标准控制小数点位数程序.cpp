#include<iostream>
#include<string>
#include<iomanip>
using namespace std;
int main()
{
    double a,b,c;
    double res;
    cin>>a>>b>>c;
    res=(a+b+c)/3.0;
    cout<<fixed<<setprecision(2)<<res<<endl;
}
