//���ļ�
int a, b,c,d;
ifstream infile;
infile.open("input.txt");      //ע���ļ���·��
infile >> a >> b>>c>>d;                   //�������ݿ�������������������
infile.close();
cout << a << b<<c<<d;

//д�ļ�
ofstream outfile;
outfile.open("output.txt");
if (outfile.is_open())
{
	outfile << a<<b<<c<<d<<"writing successfully!" << endl;
	outfile.close();
}
else
{
	cout << "���ܴ��ļ�!" << endl;
}
