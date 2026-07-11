# 🔢 表格生成器 (Table Generator)

通过 **GitHub Copilot AI 聊天**识别文本并生成结构化表格的 Web 工具。

---

## 🚀 快速开始

### 1. 打开工具
在浏览器中打开 `TableGenerator/index.html`

### 2. 选择预设
左侧面板选择预设（如「剧情对话」「战斗角色」等），预设决定了表格的列结构和解析规则。

### 3. 输入文本
在中间输入区域按预设模板格式输入结构化文本。

### 4. 发送到 Copilot
点击「🤖 发送到 Copilot」按钮 → 提示词自动复制 → 粘贴到 VS Code 的 GitHub Copilot 聊天中。

### 5. 粘贴结果
Copilot 生成表格 JSON 后 → 复制 → 在工具中点「📥 粘贴 JSON」→ 表格立即渲染。

### 6. 编辑 & 导出
- 双击单元格编辑
- 导出 CSV / JSON
- 复制表格（TSV 格式，可直接粘贴到 Excel）

---

## 📁 目录结构

```
TableGenerator/
├── index.html              # 🖥 Web UI 主界面（双击打开即可）
├── SKILL.md                # 🤖 Copilot Skill 规则（AI 解析规则）
├── .instructions.md        # 📋 Copilot 指令（自动激活）
├── README.md               # 📖 本说明文档
└── presets/                # 📋 预设库
    ├── script-dialogue.json  # 剧本对话预设（StoryData + OptionData 双表）
    ├── option-config.json    # 选项配置预设（OptionData）
    ├── f-table.json          # 战斗节点预设（F 表）
    ├── battle-character.json # 战斗角色预设
    ├── illustration.json     # 图鉴数据预设
    ├── default.json          # 默认通用预设
    └── markdown-table.json   # Markdown 表格预设
```

---

## 🔧 预设系统

预设 = **模板 + 解析脚本 + 规则描述**，类似于 Copilot 的 Skill 机制。

### 预设 JSON 结构

```json
{
  "name": "预设名称",
  "category": "分类",
  "description": "简要描述",
  "columns": ["列名1", "列名2", ...],
  "template": "输入模板（占位符格式）",
  "rules": "Markdown 格式的解析规则（AI 读取）",
  "exampleInput": "示例输入文本",
  "exampleOutput": "示例输出 JSON",
  "version": "1.0"
}
```

### 预设操作
- **新建**：点击「＋ 新建」填写预设信息
- **编辑**：鼠标悬停预设项 → 点击 ✏ 图标
- **导出**：导出预设 JSON 分享给团队
- **导入**：拖放 JSON 文件到页面，或点击「📥 导入」按钮
- **删除**：编辑预设 → 点击「🗑 删除预设」

---

## 🤖 Copilot 工作流

### 方式一：使用「发送到 Copilot」按钮
1. 在 Web UI 输入文本
2. 点击「🤖 发送到 Copilot」→ 提示词自动复制
3. 在 VS Code Copilot 聊天中粘贴
4. Copilot 根据预设规则生成 JSON
5. 复制 JSON → 在 UI 中「📥 粘贴 JSON」

### 方式二：直接对话
直接在 Copilot 中说：
> 帮我把这段文本解析成表格，列名是：节点名称、角色名称、对话内容
> ```
> 节点名称: ch1_start
> 角色: 小美
> 内容: 你好！
> ```

Copilot 会自动生成 JSON 表格数据。

### 方式三：纠错流程
1. 表格数据显示异常时，点击「🔧 纠错」
2. 提示词自动复制，粘贴到 Copilot
3. Copilot 分析并返回修正后的 JSON
4. 粘贴 JSON 更新表格

### 方式四：🔧 脚本解析（推荐）
内置 JavaScript 解析引擎，无需 Copilot 即可完成解析：
1. 选择预设（如「剧本对话解析」）
2. 在文本框粘贴 AVG 剧本文本
3. 点击「🔧 脚本解析」→ 自动解析为表格
4. 解析结果包含 `issues` 自检提醒

支持的脚本：
- **剧本对话解析**：T/TS/S/G 节点自动识别，命令同行提取（从行尾逐个提取括号），括号内容自动分流，节点名 `*` 继承，选项带锁检测
- **战斗节点配置**：F 节点战斗属性自动提取

---

## ⌨ 快捷键

| 快捷键 | 功能 |
|--------|------|
| `Ctrl+Shift+V` | 粘贴 JSON（在界面任意位置） |

---

## 🎯 预设与 Excel 的对应关系

| 预设 | 对应 Excel | 对应代码 |
|------|-----------|---------|
| 剧本对话解析 | Chapter1.xlsx (Sheet1: StoryData, Sheet2: OptionData) | `StoryTableConfig.cs` / `StoryData.cs` / `OptionData.cs` |
| 选项配置 | Chapter1.xlsx (Sheet2: OptionData) | `OptionData.cs` |
| 战斗节点配置 | - （AVG 剧情战斗节点，非战斗系统表） | - |
| 战斗角色 | BattleCharacter.xlsx | `BattleCharacterTableConfig.cs` |
| 图鉴数据 | Illustration.xlsx | `IllustrationFileDataTableConfig.cs` |

---

## 💡 提示

- 预设可以随时更新——编辑后保存即可，AI 下次解析会使用最新规则
- 表格数据存储在浏览器 `localStorage` 中，刷新页面不会丢失
- 导出 JSON 可用于备份或跨设备传输
- 支持拖放 `.json` 预设文件到页面直接导入
