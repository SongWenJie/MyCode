Centos7安装Docker

`yum -y install docker-io`

拉取镜像：

`docker pull mysql:5.7`

创建运行容器：

`docker run -p 3306:3306 --name mymysql -e MYSQL_ROOT_PASSWORD=123456 -d mysql:5.7 `

 `select user,host from user;`

![mark](http://songwenjie.vip/blog/180725/0LaCd8hg0K.png?imageslim)

`CREATE USER 'swj'@'localhost' IDENTIFIED BY '123456';`

`GRANT ALL PRIVILEGES ON *.* TO 'swj'@'localhost' WITH GRANT OPTION;`

`CREATE USER 'swj'@'%' IDENTIFIED BY '123456';`

`GRANT ALL PRIVILEGES ON *.* TO 'swj'@'%' WITH GRANT OPTION;`

![mark](http://songwenjie.vip/blog/180725/cdCcjd2H0d.png?imageslim)

![mark](http://songwenjie.vip/blog/180725/k08DFI8024.png?imageslim)

查看容器IP：

`docker inspect --format='{{.NetworkSettings.IPAddress}}' 90d4d53bf56c`



**Docker常用命令：**

1. `docker pull 镜像名称`

   拉取某个镜像到本地

2. `docker images`

   查看镜像列表

3. `docker inspect 镜像名称`

   查看某个镜像详情

4. `docker rmi 镜像名称`

   删除某个镜像。对于存在容器的镜像，首先需要删除其对应的容器才能删除该镜像。

   ​

## Docker问题(一)：bash: vi: command not found

在使用docker时，有时候我们需要编辑配置文件，需要使用vim或者vi命令,但是会发现：

`bash: vi: command not found`

因为vim没有安装，这个时候需要我们使用`apt-get install vim`命令安装vim

会出现如下问题：

```
Reading package lists... Done
Building dependency tree       
Reading state information... Done
E: Unable to locate package vim
```

此时执行`apt-get update`，然后再次执行`apt-get install vim`即可成功安装vim。



**vim命令：**

打开文件： vi /路径/文件名

插入编辑：1次Insert

替换编辑：2次Insert

上翻页：Page Up

下翻页：Page Down

保存退出：**：wq**

不保存退出：**：q!**

![mark](http://songwenjie.vip/blog/180725/D0fLi0HHC5.png?imageslim)

change master to master_host='172.17.0.2', master_user='slave', master_password='123456', master_port=3306, master_log_file='mysql-bin.000001', master_log_pos= 2398, master_connect_retry=30;



```
service mysql restart
```

```
show slave status \G;
```



测试：

```
CREATE DATABASE test;
```






**MySQL 警告信息 command line interface can be insecure** 

在命令行输入密码，就会提示这些安全警告信息。

Warning: Using a password on the command line interface can be insecure.

注: mysql -u root -prootpassword 或 mysqldump -u root -prootpassword 都会输出这样的警告信息.

## mysql

**解决方法：**

1. mysql -u root -prootpassword 改成mysql -u root -p 在输入密码即可

2. 配置 /etc/mysql/my.cnf

   vim /etc/mysql/my.cnf

   ```
   [mysql]
   user = root
   password = rootpassword
   ```

   这种方式以后直接输入mysql就可以进入数据库，不需要涉及用户名密码相关信息

## mysqldump

**解决方法：**

配置 /etc/mysql/my.cnf

vim /etc/mysql/my.cnf

```
[mysqldump]
user = root
password = rootpassword
```

修改完配置文件后, 只需要执行mysqldump 脚本就可以了。备份脚本中不需要涉及用户名密码相关信息。

## mysqladmin

**mysqladmin命令** 是mysql服务器管理任务的客户端工具，它可以检查mytsql服务器的配置和当前工作状态，创建和删除数据库，创建用户和修改用户密码等操作。

```
[mysqladmin]
user = root
password = rootpassword
```



















































# 基于Docker的Mysql主从复制搭建

## 为什么基于Docker搭建？

* 资源有限
* 虚拟机搭建对机器配置有要求，并且安装mysql步骤繁琐
* 一台机器上可以运行多个Docker容器
* Docker容器之间相互独立，有独立ip，互不冲突
* Docker使用步骤简便，启动容器在秒级别

## 利用Docker搭建主从服务器

首先拉取docker镜像,我们这里使用5.7版本的mysql：

`docker pull mysql:5.7`

然后使用此镜像启动容器，这里需要分别启动主从两个容器

**Master(主)：**

`docker run -p 3339:3306 --name mymysql -e MYSQL_ROOT_PASSWORD=123456 -d mysql:5.7`

**Slave(从)：**

`docker run -p 3340:3306 --name mymysql -e MYSQL_ROOT_PASSWORD=123456 -d mysql:5.7`

Master对外映射的端口是3339，Slave对外映射的端口是3340。因为docker容器是相互独立的，每个容器有其独立的ip，所以不同容器使用相同的端口并不会冲突。这里我们应该尽量使用mysql默认的3306端口，否则可能会出现无法通过ip连接docker容器内mysql的问题。

使用`docker ps`命令查看正在运行的容器：

![mark](http://songwenjie.vip/blog/180726/5L4Adf7clg.png?imageslim)

此时可以使用Navicat等工具测试连接mysql

![mark](http://songwenjie.vip/blog/180726/GADD1hBeH7.png?imageslim)

## 配置Master(主)

通过`docker exec -it 627a2368c865 /bin/bash`命令进入到Master容器内部，也可以通过`docker exec -it mysql-master /bin/bash`命令进入。627a2368c865是容器的id,而mysql-master是容器的名称。

`cd /etc/mysql`切换到/etc/mysql目录下，然后`vi my.cnf`对my.cnf进行编辑。此时会报出`bash: vi: command not found`，需要我们在docker容器内部自行安装vim。使用`apt-get install vim`命令安装vim

会出现如下问题：

```
Reading package lists... Done
Building dependency tree       
Reading state information... Done
E: Unable to locate package vim
```

执行`apt-get update`，然后再次执行`apt-get install vim`即可成功安装vim。然后我们就可以使用vim编辑my.cnf，在my.cnf中添加如下配置：

```
[mysqld]
## 同一局域网内注意要唯一
server-id=100  
## 开启二进制日志功能，可以随便取（关键）
log-bin=mysql-bin
```

配置完成之后，需要重启mysql服务使配置生效。使用`service mysql restart`完成重启。重启mysql服务时会使得docker容器停止，我们还需要`docker start mysql-master`启动容器。

下一步在Master数据库创建数据同步用户，授予用户 slave REPLICATION SLAVE权限和REPLICATION CLIENT权限，用于在主从库之间同步数据。

`CREATE USER 'slave'@'%' IDENTIFIED BY '123456'; `

`GRANT REPLICATION SLAVE, REPLICATION CLIENT ON *.* TO 'slave'@'%';`

![mark](http://songwenjie.vip/blog/180726/34Hjbf7Ck3.png?imageslim)

## 配置Slave(从)

和配置Master(主)一样，在Slave配置文件my.cnf中添加如下配置：

```
[mysqld]
## 设置server_id,注意要唯一
server-id=101  
## 开启二进制日志功能，以备Slave作为其它Slave的Master时使用
log-bin=mysql-slave-bin   
## relay_log配置中继日志
relay_log=edu-mysql-relay-bin  
```

配置完成后也需要重启mysql服务和docker容器，操作和配置Master(主)一致。

## 链接Master(主)和Slave(从)

在Master进入mysql，执行`show master status;`

![mark](http://songwenjie.vip/blog/180726/Kjja2jAIm4.png?imageslim)

File和Position字段的值后面将会用到，在后面的操作完成之前，需要保证Master库不能做任何操作，否则将会引起状态变化，File和Position字段的值变化。

在Slave 中进入 mysql，执行`change master to master_host='172.17.0.2', master_user='slave', master_password='123456', master_port=3306, master_log_file='mysql-bin.000001', master_log_pos= 2830, master_connect_retry=30;`

**命令说明：**

**master_host** ：Master的地址，指的是容器的独立ip,可以通过`docker inspect --format='{{.NetworkSettings.IPAddress}}' 容器名称|容器id`查询容器的ip

![mark](http://songwenjie.vip/blog/180726/7cEC8DbFI1.png?imageslim)

**master_port**：Master的端口号，指的是容器的端口号

**master_user**：用于数据同步的用户

**master_password**：用于同步的用户的密码

**master_log_file**：指定 Slave 从哪个日志文件开始复制数据，即上文中提到的 File 字段的值

**master_log_pos**：从哪个 Position 开始读，即上文中提到的 Position 字段的值

**master_connect_retry**：如果连接失败，重试的时间间隔，单位是秒，默认是60秒

在Slave 中的mysql终端执行`show slave status \G;`用于查看主从同步状态。

![mark](http://songwenjie.vip/blog/180726/2kagID6a2K.png?imageslim)

正常情况下，SlaveIORunning 和 SlaveSQLRunning 都是No，因为我们还没有开启主从复制过程。使用`start slave`开启主从复制过程，然后再次查询主从同步状态`show slave status \G;`。

![mark](http://songwenjie.vip/blog/180726/FG2ja42l86.png?imageslim)

SlaveIORunning 和 SlaveSQLRunning 都是Yes，说明主从复制已经开启。此时可以进行测试数据同步是否成功。

**主从复制排错：**

![mark](http://songwenjie.vip/blog/180726/bdcjbCF1Ii.png?imageslim)

使用`start slave`开启主从复制过程后，如果SlaveIORunning一直是Connecting，则说明主从复制一直处于连接状态，这种情况一般是下面几种原因造成的，我们可以根据 Last_IO_Error提示予以排除。

1. 网络不通

   检查ip,端口

2. 密码不对

   检查是否创建用于同步的用户和用户密码是否正确

3. pos不对

   检查Master的 Position

## 测试主从复制

测试主从复制方式就十分多了，最简单的是在Master创建一个数据库，然后检查Slave是否存在此数据库。

**Master:**

![mark](http://songwenjie.vip/blog/180726/Kg1EB8f1HL.png?imageslim)

**Slave:**

![mark](http://songwenjie.vip/blog/180726/4Ibh3CF9D3.png?imageslim)

问题：

1. 主从复制 主库之前的数据怎么办
2. 从库停掉了怎么办
3. 主主复制
4. 一主多从（已解决）





# MySQL主从复制——主库已有数据的解决方案

在上篇文章中我们介绍了[基于Docker的Mysql主从搭建](https://www.cnblogs.com/songwenjie/p/9371422.html)，一主多从的搭建过程就是重复了一主一从的从库配置过程，需要注意的是，要保证主从库my.cnf中**server-id**的唯一性。搭建完成后，可以在主库`show slave hosts`查看有哪些从库节点。

![mark](http://songwenjie.vip/blog/180727/D59bemF1kd.png?imageslim)

我们来简单了解一下Mysql主从复制的过程：

(1)  master将改变记录到二进制日志(binary log)中（这些记录叫做二进制日志事件，binary log events）； 
(2)  slave将master的binary log events拷贝到它的中继日志(relay log)； 
(3)  slave重放中继日志中的事件，将改变反映它自己的数据。

![mark](http://songwenjie.vip/blog/180727/4BBfe2EJLc.png?imageslim)



## MySQL主从复制——主库已有数据的解决方案

由单机架构切换到一主一从或一主多从，在增加从库节点前，主库可能已经运行过一段时间，这种情况在实际业务中很常见。那么如何应对开启主从复制前主库有数据的场景呢？

第一种方案是选择忽略主库之前的数据，不做处理。这种方案只适用于不重要的可有可无的数据，并且业务上能够容忍主从库数据不一致的场景。

第二种方案是对主库的数据进行备份，然后将主数据库中导出的数据导入到从数据库，然后再开启主从复制，以此来保证主从数据库数据一致。

我们来详细看一下第二种方案的处理：

### 查看主数据库已有的数据库

我们在主数据库准备了一个**TEST1**库，并且在其中准备一张数据表TEST和几条测试数据。

![mark](http://songwenjie.vip/blog/180727/hk6cFfb7ke.png?imageslim)

![mark](http://songwenjie.vip/blog/180727/bFfDjAj41f.png?imageslim)

### 使用Docker创建从数据库

`docker run -p 3346:3306 --name mysql-slave4 -e MYSQL_ROOT_PA SSWORD=123456 -d mysql:5.7`

![mark](http://songwenjie.vip/blog/180727/JgGFecf6jl.png?imageslim)

### 锁定主数据库

锁定主数据库，只允许读取不允许写入，这样做的目的是防止备份过程中或备份完成之后有新数据插入，导致备份数据和主数据数据不一致。

 `mysql> flush tables with read lock;`

![mark](http://songwenjie.vip/blog/180727/EFbLALeK8a.png?imageslim)

![mark](http://songwenjie.vip/blog/180727/4El4l5l62D.png?imageslim)

### 查询主数据库状态，并记下FILE及Position的值

`mysql>show master status;`

![mark](http://songwenjie.vip/blog/180727/L40Gh5hFdE.png?imageslim)

### 备份主数据库

退出mysql终端，执行docker mysql备份命令

`docker exec [CONTAINER] /usr/bin/mysqldump -u username --password=xxx [DATABASE] > backup.sql`

我们这里只需要备份**TEST1**数据库，若要备份全部数据库,[DATABASE]处使用`--all-databases`。

![mark](http://songwenjie.vip/blog/180727/8kfC42Ba9J.png?imageslim)

此时报出`Warning: Using a password on the command line interface can be insecure.`，这是因为我们在命令行输入了密码，所以会有安全警告信息。解决方案是在/etc/mysql/my.cnf中加入如下配置：

```
[mysqldump]
user = root
password = rootpassword
```

修改完配置文件后,再次执行备份命令不需要涉及用户名密码相关信息。

![mark](http://songwenjie.vip/blog/180727/2Eeg3fHiH5.png?imageslim)

### 主数据库备份数据导入从数据库

```
cat backup.sql | docker exec -i [CONTAINER] /usr/bin/mysql -u username --password=xxx [DATABASE]
```

和上一步一样，我们也需要在从数据库配置用于备份的用户名和密码信息。

![mark](http://songwenjie.vip/blog/180727/jikB1DLIKh.png?imageslim)

需要先在从数据库建立一个同名数据库，才能导入主数据库备份数据。切换到从数据库执行`CREATE DATABASE TEST1;`，然后再次导入主数据备份数据。

![mark](http://songwenjie.vip/blog/180727/LBH1LaJe87.png?imageslim)

此时备份数据导入已完成，可以在从数据库进行数据验证。

![mark](http://songwenjie.vip/blog/180727/h6akj9F724.png?imageslim)

### 配置从数据库

`docker exec -it mysql-slave4 /bin/bash`

`cd /etc/mysql`

![mark](http://songwenjie.vip/blog/180727/JaCdkIaijJ.png?imageslim)

`vi my.cnf`，加入以下配置，注意**server-id**要保证唯一：

![mark](http://songwenjie.vip/blog/180727/093e935lEi.png?imageslim)

`service mysql restart`重启mysql服务，这会使得mysql服务所在的docker容器停止

`docker start mysql-slave4`启动docker容器



### 配置主从链接

切换到从数据库，执行`change master to master_host='172.17.0.2', master_user='slave', master_password='123456', master_port=3306, master_log_file='mysql-bin.000001', master_log_pos= 4952, master_connect_retry=30;`，关于这个命令在上一篇博客中有详细介绍。

启动主从复制`start slave; `，此时查看从库状态`show slave status \G;`，若是SlaveIORunning 和 SlaveSQLRunning 都是Yes，说明开启主从复制过程成功。

![mark](http://songwenjie.vip/blog/180727/aKhklabDkC.png?imageslim)



### 解锁主数据库

切换回主数据库的终端，进行表解锁操作。

`unlock tables;`

![mark](http://songwenjie.vip/blog/180727/mLK3lkbCFl.png?imageslim)

### 测试主从复制

在主数据库插入一条测试数据

![mark](http://songwenjie.vip/blog/180727/Kjki8Lh4fh.png?imageslim)

切换到从数据库，查询测试数据，说明主从复制成功。

![mark](http://songwenjie.vip/blog/180727/LBic4eFm8h.png?imageslim)



### 总结

应该尽可能优化流程，减少锁表时间。

尽可能减少锁表范围，只锁定相关的数据库。





一主多从，发现有一个从库数据同步不正常。

`show slave status \G`查看从库状态发现是因为sql执行出错导致的，主库删除test数据库，操作同步到从库，但是从库并没有test数据库，所以导致了此错误。一定要注意主库从库数据结构的一致性。

![mark](http://songwenjie.vip/blog/180727/mfd2L4dcHb.png?imageslim)