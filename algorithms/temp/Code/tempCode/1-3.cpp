#include <iostream>
#include <string>

using namespace std;

int convert(int n, string s)
{
    int ret=0, pow = 1;
    for(int i = s.length()-1; i>=0; i--)
    {
        int temp;
        if(s[i]>='A' && s[i]<='F') temp = s[i]-'A'+10;
        else temp=s[i]-'0';

        if(temp >= n) return -1;

        ret += pow * temp;
        pow *= n;
    }
    return ret;
}
int calculate(int x, int y, string s)
{
    int candidateX, candidateY;
    for(int i=1; i<s.length(); i++)
    {
        string sX = s.substr(0, i);
        string sY = s.substr(i, s.length()-i);
        if(sY[0] == '0') continue;
        candidateX = convert(x, sX);
        if(candidateX == -1) continue;
        candidateY = convert (y, sY);
        if(candidateY == -1) continue;

        if(candidateX == candidateY) return candidateX;
    }
    return -1;
}

int main()
{
    int t;
    cin>>t;
    while(t)
    {
        t--;
        int x, y;
        string s;
        cin>>x>>y>>s;
        cout<<calculate(x, y, s)<<endl;
    }
    return 0;
}
