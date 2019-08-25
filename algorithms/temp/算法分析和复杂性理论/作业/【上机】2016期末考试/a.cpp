#include <iostream>
#define MAXN 110
using namespace std;
int row, col;
int a[MAXN][MAXN];
int go[2]={1, -1};

int main()
{
    cin>>row>>col;
    for(int i=0; i<row; i++)
    {
        for(int j=0; j<col; j++)
        {
            cin>>a[i][j];
        }
    }
    for(int j=0; j<col; j++)
    {
        int temp = j;
        for(int i=0; i<row; i++)
        {
            if(temp >=0 && temp < col && i>=0 && i< row)
            {
                cout<<a[i][temp]<<endl;
            }
            temp -=1;
        }
    }
    for(int i = 1; i<row; i++)
    {
        int temp =i;
        for(int j=col-1; j>=0; j--)
        {
            if(temp >=0 && temp < row && j>=0 && j< col)
            {
                cout<<a[temp][j]<<endl;
            }
            temp +=1;
        }
    }
    return 0;
}
