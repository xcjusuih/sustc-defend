# sustc-defend

### Top Priority:  完善游戏流程

### 模型 -HE
 - [x] 模型基类-BaseRobot
      - int hp
 - [x] 玩家模型-RobotPlayer : BaseRobot
      - List<WeaponBsdr> Weapons;
      - float walkspeed = 10;
      - int CurWeaponIdx = 0;
      - Animator animator
      - CharacterController cc
      - Transform Hand // var new_weapon = GameObject.Instantiate(weapon, Hand);//挂接武器
      - void AddWeapon(GameObject weapon)
 - [x] 防御塔模型 -KOU + WU
      - int hp
      - int cost
      - int attack_range(肉设0)
      - int attack_damage
      - int attack_speed
      - int level
 - [x] 枪械模型 
 - [x] 武器切换
 - [ ] 怪物模型 -KOU
      - int attack
      - int attack_range
      - int attack_speed
      - int hp
      - int move_speed
 - [x] 场景建模  （已初步实现简单场景）
 
### UI
- [x] UI界面 -WU + CHEN
- [x] 场景跳转 -WU  //跳转到battlescene
- [ ] 商店 -CHEN
    -流程： 两拨中间暂停->选择位置->weapon tower-> 展开 选择 -> 购买 
    -金钱： 全局变量

### 逻辑
- [x] 怪物生成，寻路逻辑 -KOU
- [x] 防御塔攻击逻辑 -KOU + WU
- [x] 子弹碰撞，扣血 -HE
- [ ] 金钱, 血包， 加攻, 掉落 -KOU

### 接入框架 -WU


### 美化
- [ ] 场景美化 -HE
- [ ] 模型美化(optional) -CHEN

### 游戏体验
- [ ] 故事文案 -HE
- [ ] 动画流畅度 -各自改各自做的模型
- [ ] 数据平衡 -CHEN
- [ ] 丰富敌人种类
- [ ] 丰富防御塔种类
- [ ] 联机游戏(optional)
