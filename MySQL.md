## Myisam 和 InnoDB 的区别？

其实关于这两个最常用的存储引擎，无非就是看场景，其实没有绝对的好与坏，不要教条主义，适合自己业务的就是最好的。

- Myisam：表级锁，不支持事务，读性能更好，读写分离中做读（从）节点。老版本 MySQL 的默认存储引擎。表的存储分为三个文件：frm表格式，MYD/MYData 数据文件，myi 索引文件。
- InnoDB：有条件的行级锁，支持事务，更适合作为读写分离中的写（主）节点。新版本 MySQL（5.7开始）的默认存储引擎。



## 双机热备

Mysql双机互为热备方案。在进行讨论的时候一定要考虑到很多的因素，其中在各种环境下应用的时候需要格外的引起注意。按照上面说的复制模式，一般有2中方案。1，双机热备它的工作原理是使用两台服务器，一台作为主服务器（Master或者Active），运行应用系统来提供服务。另一台作为备机，安装完全一样的应用系统，但处于待机状态（Slave或者叫Standby）。当Master服务器出现故障时，通过软件诊测将Slave机器激活，保证应用在短时间内完成恢复正常使用。2，另外一种形式就是，双机互备方式则是在双机热备的基础上，两个相对独立的应用在两台机器同时运行，但彼此均设为备机，当某一台服务器出现故障时，另一台服务器可以在短时间内将故障服务器的应用接管过来，从而保证了应用的持续性，这种方式实际上是双机热备方案的一种应用。





# 通过EXPLAIN分析低效SQL的执行计划

我们可以通过**EXPLAIN**命令获取MySQL如何执行SELECT语句的信息，包括在SELECT语句执行过程中表如何连接和连接的顺序。

![mark](http://songwenjie.vip/blog/180728/FA4eKFJgff.png?imageslim)

分别对**EXPLAIN**命令结果的每一列进行说明：

* **select_type**:表示SELECT的类型，常见的取值有：

  |   类型   |               说明                |
  | :------: | :-------------------------------: |
  |  SIMPLE  |   简单表，不使用表连接或子查询    |
  | PRIMARY  |       主查询，即外层的查询        |
  |  UNION   | UNION中的第二个或者后面的查询语句 |
  | SUBQUERY |         子查询中的第一个          |

* **table**:输出结果集的表（表别名）

* **type**:表示MySQL在表中找到所需行的方式，或者叫访问类型。常见访问类型如下，从左到右，性能由差到最好：

  |   ALL    |   index    |    range     |      ref       |    eq_ref    |     const,system     |       NULL       |
  | :------: | :--------: | :----------: | :------------: | :----------: | :------------------: | :--------------: |
  | 全表扫描 | 索引全扫描 | 索引范围扫描 | 非唯一索引扫描 | 唯一索引扫描 | 单表最多有一个匹配行 | 不用扫描表或索引 |

  查看`customer` `payment` `film` `film_text`表结构和索引情况：

  ![mark](http://songwenjie.vip/blog/180728/7kKAE55a9i.png?imageslim)

  ![mark](http://songwenjie.vip/blog/180728/af0kJmCk1j.png?imageslim)

  ![mark](http://songwenjie.vip/blog/180728/KlLa7c3hK2.png?imageslim)

  ![mark](http://songwenjie.vip/blog/180728/4l5ej5368C.png?imageslim)

  ​

  1. **type=ALL，全表扫描，MySQL遍历全表来找到匹配行**

     一般是没有where条件或者where条件没有使用索引的查询语句

     `EXPLAIN SELECT * FROM customer WHERE active=0;`

     ![mark](http://songwenjie.vip/blog/180728/Ledh860lh0.png?imageslim)

  2. **type=index，索引全扫描，MySQL遍历整个索引来查询匹配行，并不会扫描表**

     一般是查询的字段都有索引的查询语句

     `EXPLAIN SELECT store_id FROM customer;`

     ![mark](http://songwenjie.vip/blog/180728/H1c5I109hJ.png?imageslim)

  3. **type=range，索引范围扫描，常用于<、<=、>、>=、between等操作**

     `EXPLAIN SELECT * FROM customer WHERE customer_id>=10 AND customer_id<=20;`

     ![mark](http://songwenjie.vip/blog/180728/AFjb9ai88a.png?imageslim)

     注意这种情况下比较的字段是需要加索引的，如果没有索引，则MySQL会进行全表扫描，如下面这种情况，create_date字段没有加索引：

     `EXPLAIN SELECT * FROM customer WHERE create_date>='2006-02-13' ;`

     ![mark](http://songwenjie.vip/blog/180728/EDDiFef0Ee.png?imageslim)

  4. **type=ref，使用非唯一索引或唯一索引的前缀扫描，返回匹配某个单独值的记录行**

     `store_id`字段存在普通索引（非唯一索引）

     `EXPLAIN SELECT * FROM customer WHERE store_id=10;`

     ![mark](http://songwenjie.vip/blog/180728/HFmh56bafe.png?imageslim)

     **ref**类型还经常会出现在join操作中：

     **customer**、**payment**表关联查询，关联字段`customer.customer_id`（主键），`payment.customer_id`（非唯一索引）。表关联查询时必定会有一张表进行全表扫描，此表一定是几张表中记录行数最少的表，然后再通过非唯一索引寻找其他关联表中的匹配行，以此达到表关联时扫描行数最少。

     ![mark](http://songwenjie.vip/blog/180728/C03A0GmHb8.png?imageslim)

     因为**customer**、**payment**两表中**customer**表的记录行数最少，所以**customer**表进行全表扫描，**payment**表通过非唯一索引寻找匹配行。

     `EXPLAIN SELECT * FROM customer customer INNER JOIN payment payment ON customer.customer_id = payment.customer_id;`

     ![mark](http://songwenjie.vip/blog/180728/7egBmA56C2.png?imageslim)

  5. **type=eq_ref，类似ref，区别在于使用的索引是唯一索引，对于每个索引键值，表中只有一条记录匹配**

     **eq_ref**一般出现在多表连接时使用primary key或者unique index作为关联条件。

     **film、film_text**表关联查询和上一条所说的基本一致，只不过关联条件由非唯一索引变成了主键。

     `EXPLAIN SELECT * FROM film film INNER JOIN film_text film_text ON film.film_id = film_text.film_id;`

     ![mark](http://songwenjie.vip/blog/180728/4H03LBl4Lj.png?imageslim)

  6. **type=const/system，单表中最多有一条匹配行，查询起来非常迅速，所以这个匹配行的其他列的值可以被优化器在当前查询中当作常量来处理**

     **const/system**出现在根据主键primary key或者 唯一索引 unique index 进行的查询

     ![mark](http://songwenjie.vip/blog/180728/6h95c9ag8G.png?imageslim)

     ![mark](http://songwenjie.vip/blog/180728/J4klb0cd97.png?imageslim)

     ![mark](http://songwenjie.vip/blog/180728/h7I5f0JJJa.png?imageslim)

  7. 是是是


 ref列显示使用哪个列或常数与key一起从表中选择行。