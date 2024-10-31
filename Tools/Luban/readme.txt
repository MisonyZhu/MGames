WORKSPACE：“.”代表当前目录，就是gen.bat所在的Minitemplate文件夹。不需要改动，在给出的gen.bat中也用不到，所以可以不管。
LUBAN_DLL：就是我们前面拷贝过来的Tools\Luban文件夹中Luban.dll的地址。
CONF_ROOT：配置文件的根目录，也就是Datas、Defines文件夹所在的目录。
outputCodeDir：生成的代码，所放的目录。我直接指定到我unity工程所在的目录。
outputDataDir：excel表格转化成json文件后，存放的位置。我存放在unity项目的根目录下，这样代码中就可以直接读取了