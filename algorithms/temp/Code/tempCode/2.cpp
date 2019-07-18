#include <iostream>
#include <string>
using namespace std;

int main()
{
    int t;
    int a, b, n;
    int guess_num;
    string out_str;

    cin>>t;
    cin>>a>>b;
    cin>>n;
    for (int i=0; i<t; i++) {
        while(1)
        {
            guess_num = (a+b)/2;
            cout<<guess_num;
            cin>>out_str;
            if(out_str == "CORRECT") {
                break;
            } else if (out_str == "TOO_SMALL") {
                a = guess_num + 1;
            } else if (out_str == "TOO_BIG") {
                b = guess_num - 1;
            } else {
                return -1;
            }
        }
    }
}
