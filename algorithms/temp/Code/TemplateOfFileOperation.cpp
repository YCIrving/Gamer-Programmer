//读文件
int a, b,c,d;
ifstream infile;
infile.open("input.txt");      //注意文件的路径
infile >> a >> b>>c>>d;                   //两行数据可以连续读出到变量里
infile.close();
cout << a << b<<c<<d;

//写文件
ofstream outfile;
outfile.open("output.txt");
if (outfile.is_open())
{
	outfile << a<<b<<c<<d<<"writing successfully!" << endl;
	outfile.close();
}
else
{
	cout << "不能打开文件!" << endl;
}
