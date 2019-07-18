/*
算法没什么难的，重点就是优化，可以不用A和B完全搜一遍
B从A开始搜即可，如果A和B不同，让ans+=2。
*/

#include<iostream>
#include<math.h>
#include<fstream>
using namespace std;
int num[10]={6,2,5,5,4,5,6,3,7,6};
int getNum(int n)
{
    int output=0,temp;
    if(n==0)
        return 6;
    for(int i=0;;i++)
    {
        temp=n/(int)pow(10,i);
        if(temp!=0)
            {
                temp=temp%10;
                output+=num[temp];
            }
        else
            break;
    }
    return output;
}
int main()
{

    int n;
    int num1,num2,num3,ans=0;
//    ifstream infile;
//    infile.open("matches.in");
//    infile>>n;
    cin>>n;
    n=n-4;
    for(int i=0;i<1000;i++)
    {
        num1=getNum(i);
        for(int j=i;j<1000;j++)
        {
            num2=getNum(j);
            int temp=i+j;
            num3=getNum(temp);
            if((num1+num2+num3)==n)
            {
                //cout<<i<<"+"<<j<<"="<<temp<<endl;
                //cout<<num1<<"+"<<num2<<"+"<<num3<<"="<<n<<endl<<endl;
                if(i==j)
                    ans++;
                else
                    ans+=2;
            }
        }
    }
//    ofstream outfile;
//    outfile.open("matches.out");
//    outfile<<ans;
    cout<<ans;
}
