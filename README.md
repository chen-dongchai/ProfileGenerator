## ProfileGenerator —— Revit 复杂轮廓生成插件

> **V1.0.0** · 一键生成镂空图案轮廓，导出 RFA / DWG / 直接开洞

---

## 📌 简介

ProfileGenerator 是一个 Revit 插件，帮助你在族编辑器中快速生成带有镂空图案的复杂二维轮廓。支持多种外部环形状、内部图案类型和排列方式，并可导出为 RFA 族文件或 DWG 文件，或直接应用到项目中的墙体上开洞。

**适用于 Revit 2022 | C# | Revit API**

---

## ✨ 功能特性

- **外部环**：矩形、圆形
- **内部图案**：矩形、圆形、菱形、星形（角数可调）
- **排列方式**：网格排列、交错排列（行偏移 / 列偏移）、Voronoi 图（随机多边形）
- **导出方式**：RFA 族文件、DWG 文件
- **直接应用到墙**：在选中的墙面上生成开洞

---

## 📦 安装

1. 从 [Releases](https://github.com/你的用户名/ProfileGenerator/releases) 下载最新的 `.msi` 安装包
2. 双击运行，按提示完成安装
3. 重启 Revit，在 **附加模块** 选项卡中找到 `ProfileGenerator`

---

## 🚀 快速上手

1. 在 Revit 中打开一个族文件（.rfa）或项目文件
2. 点击附加模块 → ProfileGenerator
3. 选择外部环形状、内部图案和排列方式
4. 点击 **生成**，预览轮廓
5. 导出为 RFA / DWG，或点击 **应用到墙** 在项目中直接开洞

---

## 📄 许可证

MIT License

## 🛠️ 技术栈

- C# / .NET Framework 4.8
- Revit API 2022
- WinForms
- Visual Studio Installer Projects

---

## 📧 联系

- 邮箱：3368947934@qq.com
- GitHub：[chen-dongchai](https://github.com/chen-dongchai)

---

## 📄 许可证

MIT License — 详见 [LICENSE](LICENSE) 文件
