#include <iostream>
#include <stdlib.h>
#include <stdio.h>

using namespace std;

double calculation()
{
    string s;
    cin>>s;
    if (s.length() == 1 )
    {
        switch (s[0])
        {
        case '+':
            return calculation() + calculation();
        case '-':
            return calculation() - calculation();
        case '*':
            return calculation() * calculation();
        case '/':
            return calculation() / calculation();
        default:
            return atof(s.c_str());
        }
    }
    else
    {
        return atof(s.c_str());
    }
}

int main()
{
    double ans=0;
    ans = calculation();
    printf("%f\n",ans);
}
