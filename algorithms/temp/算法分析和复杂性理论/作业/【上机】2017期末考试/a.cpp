#include <iostream>
#include <memory.h>
using namespace std;

char mat[200][200];

int main()
{
    memset(mat, '\0', sizeof(mat));
    int c;
    cin>>c;
    string s;
    getline(cin, s);
    getline(cin, s);
    int i=0, j=0, k=0;
    int tag =0;
    for(; i<s.length(); i++)
    {

        if( tag == 0 && k == c)
        {
            tag = 1;
            k = c-1;
            j++;
        }

        if(tag ==1 && k ==-1)
        {
            tag =0;
            k = 0;
            j++;
        }
        mat[j][k]= s[i];
        if(tag ==0)
        {
            k++;
        }
        if(tag ==1)
        {
            k--;
        }
//         cout<<j<<endl;
    }

    for(int q=0; q<c; q++)
    {
        for(i=0; i<=j; i++)
        {
            cout <<mat[i][q];
        }
    }
    return 0;

}
