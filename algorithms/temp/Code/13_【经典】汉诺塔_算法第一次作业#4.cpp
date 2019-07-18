#include <iostream>
#include <string>
using namespace std;
int hanoi(char source, char transit, char target, int n)
{
    if(n==0)
    {
        //cout<<n<<':'<<source<<"->"<<target<<endl;
        return 0;
    }
    else
    {
        hanoi(source, target, transit, n-1);
        cout<<n<<':'<<source<<"->"<<target<<endl;
        hanoi(transit, source, target, n-1);
    }
}
int main()
{
    int n;
    char a,b,c;
    cin>>n>>a>>b>>c;
    hanoi(a, b, c, n);
    return 0;
}
