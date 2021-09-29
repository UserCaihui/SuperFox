# 文档开头
## 1 设计目标

《超级玛丽》是一代人的童年经典游戏，其易于上手的操作和丰富的关卡元素让大家津津乐道，我们组计划用一个小狐狸素材包，制作一个《超级狐狸》，通过一定程度的模仿、复刻来致敬这个经典的游戏。

## 2 概要设计

### 2.1 功能实现

该游戏是2d的闯关游戏，其实现的是让玩家操控角色（小狐狸）在地图上进行移动、跳跃，然后收集物品和消灭敌人。主要要实现的功能有：  

（1）小狐狸的移动和跳跃  

（2）敌人的移动  

（3）收集物品时计分  

（4）暂停游戏，期间能更改游戏设置或退出游戏  

（5）小狐狸进行一些动作或触发一些事件时，加入音效。  

### 2.2 功能模块图

![alt 功能模块图](https://github.com/UserCaihui/SuperFox/blob/main/%E5%8A%9F%E8%83%BD%E6%A8%A1%E5%9D%97%E5%9B%BE.jpg)


## 3 详细设计

### 3.1 类图
![alt 类图](https://github.com/UserCaihui/SuperFox/blob/main/%E7%B1%BB%E5%9B%BE.jpg)
图中各个类的说明如下：

#### 3.1.1 Scroll：实现地图背景的左右滚动
- left：确定左边界
- right：确定右边界
- distance:左右边界之间的距离
- Start():在游戏开始时获取边界和距离
- Update():在游戏运行过程中更新左右边界的坐标，达到背景左右平移的效果。

#### 3.1.2 Shift：实现地图背景的上下滚动
- up：确定上边界
- down：确定下边界
- distance:上下边界之间的距离
- Start():在游戏开始时获取边界和距离
- Update():在游戏运行过程中更新上下边界的坐标，达到背景上下平移的效果。

#### 3.1.3 PlayerControllor：操作人物
- speed：速度
- jumpForce：跳跃力度
- isGround:是否在地面
- isCroch:是否趴下
- isHurt:是否受伤
- jumpCount:跳跃次数，实现二段跳
- cherry:收集的樱桃数目
- Movement():实现小狐狸移动的函数。
- Jump():实现小狐狸跳跃的函数。
- SwitchAnim():切换小狐狸动画表现的函数。
- OnTriggerEnter2D():判断小狐狸与其他触发器的碰撞，基于此实现收集物品。
- OnCollisionEnter2D():判断小狐狸与其他碰撞器的碰撞，基于此实现与敌人战斗。
- Restart():小狐狸死后游戏重置。

#### 3.1.4 SceneManeger：实现场景（关卡）切换
- Restart():游戏重置，回到主菜单。
- Next():下一关。
- Exit():退出游戏。

#### 3.1.5 Audio：播放背景音乐和音效。
- clip:获取播放的音乐片段。
- audioSouece:音乐播放器。
- Play():播放音乐。
- Destory():停止播放。

#### 3.1.5 Enemy：怪物类
- anim:获取怪物的动画。
- Start():开始游戏时播放怪物的动画。
- Explosion():被消灭时爆炸。
- Death（）：被消灭后销毁对象。

#### 3.1.6 Enemy_Frog：青蛙
- speed:移动速度
- jumpForce:跳跃力度
- Movement():实现青蛙边移动边跳跃的函数。
- AnimSwitch():切换青蛙的动画效果。

#### 3.1.7 Enemy_Eagle：老鹰
- speed:移动速度
- Movement():老鹰移动。

#### 3.1.8 Enemy_Eagle：猫
- speed:移动速度
- Movement():猫移动。

#### 3.1.9 Item：物品类
- anim:获取物品的动画
- Play():游戏开始时播放物品的动画（摇晃或闪烁）。
- Destory():被获取后销毁对象。

#### 3.1.10 Cherry：樱桃
- score:樱桃的分数
- Increase(int):增加分数。

#### 3.1.10 Gem：宝石
- score:宝石的分数
- Increase(int):增加分数。


### 3.2 活动图
![alt 活动图](https://github.com/UserCaihui/SuperFox/blob/main/%E6%B4%BB%E5%8A%A8%E5%9B%BE.jpg)


