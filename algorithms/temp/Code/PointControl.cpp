/*该程序示例旨在说明在程序中如何控制小数点的输出，
引用iomanip头文件，然后在输出前加上std::fixed即可不用科学计数法输出，
然后用setprecision可以设置显示的小数点位数，默认为6位*/



#include<iostream>
#include <iomanip>
using namespace std;
int main()
{
    double a=0;
    cin>>a;
    cout<<a<<endl;
    cout<<std::fixed<<a<<endl;
    cout<<std::fixed<<setprecision(0)<<a<<endl;//会四舍五入
    cout<<std::fixed<<setprecision(2)<<a;
    return 0;
}


