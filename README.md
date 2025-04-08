# DRHPS-Translation 仓库

用于存放 **Deltarune - Hypothesis** 汉化项目的所有重要文件。  
当前仓库适用于游戏版本：**2025-04-02发布的愚人节预览版**。

## 目录结构

### `code-csharp` 目录

存放通过对 `DRHPS/Managed/Assembly-CSharp.dll` 进行反编译得到的全部 C# 源代码，供使用 DnspyEx 修改硬编码文本和逻辑时参考。

- `en_US/`：完整的原始代码，用于参考算法和结构；
- `zh_CN/` 和 `zh_TW/`：仅包含有**硬编码文本或打字机逻辑改动**的文件，未修改的代码不会放入。

> 💡 使用 DnspyEx 时，可通过搜索类名或字符串快速定位要修改的文件。
>
> ⚠️ 注意：由于反编译并非完全无损，可能出现报错，建议尽量 **小范围修改**（只修改方法，不修改整个类）。

---

### `dll-backup` 目录

备份原始与汉化后的 `Assembly-CSharp.dll` 文件，用于版本对比。

- `en_US/`：原始DLL  
- `zh_CN/` / `zh_TW/`：当前最新的汉化版本DLL

---

### `font` 目录

存放汉化过程中用到的字体资源，用于通过 UABEA 替换字体。

- `original/`：游戏原始字体
- `processed/`：已嵌入中文字符的字体
- `ttf-otf/`：可在其他项目中使用的独立字体文件

在基于 TextMeshPro 的 Unity 项目中，字体资源包含以下三部分：

1. `Font*.json`：字体配置信息；
2. `Font Atlas*.json` 与 `Font Atlas*.png`：字图（字体贴图）；
3. `Font Material*.json`：材质信息（通常无需修改）。

导入方法：
- `.json`：通过 UABEA 的 **Import Dump (JSON)** 导入；
- `.png`：使用 UABEA 的 **Plugin - Edit Texture** 进行替换。

> ⚠️ 建议打开 UABEA 后 **导入两次 JSON 文件**，第一次可能会失败。

---

### `image` 目录

记录所有需要汉化的贴图资源，以及当前的翻译进度。

贴图导入方式：
1. 使用 UABEA 批量打开 `level0 ~ level50`；
2. 按 Type 排序，选中所有 `Texture2D`；
3. 使用 **Plugin - Batch Import**，选择对应语言的目录：

```plaintext
images/zh_CN/import   # 导入简体中文贴图
images/zh_TW/import   # 导入繁体中文贴图
images/en_US/import   # 还原英文贴图
```

其他子目录用于**标注翻译进度和贴图状态**，具体格式见：`images/标注.txt`。

---

### `strings` 目录

存放通过脚本提取的非硬编码文本（用于翻译和对照）。

- 分为两类：
  - `dialogue/`：对话文本（Weblate 中的 textmeshpro 模块）
  - `mtext/`：UI文本、日志文本等（Weblate 中的 mtext 模块）

命名规范说明：
- `*.json`：原始英文文本，键为 `场景-导入文件-PathID-编号`，值为英文文本；
- `*-zh_CN.json`：简体中文翻译（空值表示未翻译）；
- `*-newtext.json`：更新版本中新出现的文本；
- `*-notfound.json`：更新版本中消失的文本，可用于找回已翻译内容。

👉 所有 json 可通过运行根目录下 `translation-script.py` 中的 `update_translation_json()` 自动生成。

---

### `tasks` 目录

收录汉化过程中的各种标准、规范和注意事项，目前仍在整理中，欢迎协助补充。

---

### `text` 目录

存放可导入游戏内的最终翻译文本（由脚本生成）。结构与 `text/en_US` 保持一致。

导入流程：
1. 运行脚本中 `import_translation()` 函数；
2. 使用 UABEA 批量打开 `level0 ~ level50`；
3. 按 Type 排序，选中所有 **MonoBehaviour** 项；
4. 使用 **Import Dump - JSON**，选择 `text/zh_CN` 或 `text/zh_TW` 文件夹。

> ⚠️ UABEA 导入 MonoBehaviour 时首次导入可能失败，建议尝试两次以确保成功。

---

### `translation-tools/weblate` 目录

存放从 Weblate 下载的最新翻译内容（ZIP解压后）。

使用说明：
1. 登录 Weblate，点击项目主页下载翻译 ZIP；
2. 解压其中的 `drhps/` 文件夹至此目录；
3. 运行脚本中的 `initialize_translation_json()` 与 `update_translation_json()` 更新本地 `strings` 中的翻译文本。

## 使用方法
1. **准备工作**

   - 安装依赖：
     使用Python的包管理工具安装zhconv库，用于将简体中文转换为繁体中文。

     确保当前目录结构完整，尤其是`strings`、`text/en_US`、`translation-tools/weblate`等目录已经存在。
     
     > （一般应该都是存在的，写这段是以防万一）
     > 如果不存在，请创建好对应的文件夹，使用 UABEA 打开游戏资源（level0 ~ level50），按 `Type` 排序，选择所有 `Monobehaviour` 类型资源，并用 `Export dump - JSON` 导出到 `text/en_US`

   - 获取Weblate翻译文本：
     从Weblate页面下载翻译 ZIP 文件（选择“下载翻译ZIP”），将其中的 `drhps` 目录的所有子文件夹解压至 `translation-tools/weblate` 下。

1. **运行脚本**

   在项目根目录运行主脚本：

   ```bash
   python translation-script.py
   ```

   此操作会依次执行以下内容：

   ### （1）生成原始英文文本对照表

   ```python
   initialize_translation_json()
   ```

   - 将 `text/en_US` 目录下提取的原始英文文本，整理为 `strings/mtext.json` 和 `strings/dialogue.json` 两个文件。
   - 其中 `mtext.json` 包含UI元素、系统文本等，`dialogue.json` 则为角色对白等内容。

   ### （2）更新翻译文本

   ```python
   update_translation_json()
   ```

   - 读取 `translation-tools/weblate` 中的翻译文件，合并进原始json，生成带翻译的 `strings/*-zh_CN.json` 文件。

   ### （3）同步翻译文件

   ```python
   os.system("cp -r ...")
   ```

   - 将更新后的翻译文件复制覆盖回 `translation-tools/weblate` 目录，保持两个目录同步。

   ### （4）生成用于导入的文本文件

   ```python
   import_translation()
   ```

   - 根据英文原文和翻译文本，生成可用于UABEA导入的文本，输出至 `text/zh_CN/`。

   ### （5）生成繁体中文文本

   ```python
   convert_zh_to_tw()
   ```

   - 自动将简体中文翻译文件转为繁体，输出至 `text/zh_TW/`。

2. **导入游戏**

   - 使用 UABEA 打开游戏资源（level0 ~ level50），按 `Type` 排序，选择所有 `Monobehaviour` 类型资源；
   - 使用 `Import Dump - JSON` 批量导入 `text/zh_CN` 或 `text/zh_TW` 下的文本；
   - 若首次导入失败（导入后使用 Edit Data 报错），请立即**重新导入一次**以确保成功。

## 开发者注意事项

- **尽量小范围修改DLL**  
  在使用Dnspy修改`Assembly-CSharp.dll`时，建议优先修改**方法（Method）级别**而非整个类（Class），避免因逆向还原的不完整性导致报错。

- **类名或方法找不到？**
  使用DnspyEx的全局搜索功能，查找关键词（如字符串文本、类名、变量名等）定位相关代码。

- **字体资源三件套必须同时导入**
  修改字体时，务必通过UABEA同时导入 `.json`、`.png` 的三件套（Font.json, Font Atlas.json, Font Atlas.png），否则游戏可能无法正常显示汉字。

- **使用批量导入时注意文件名与格式**
  UABEA的 `Batch Import` 识别文件名，需保持贴图或文本文件结构、命名与原始资源一致。

---

## 常见问题

### Q: 为什么第一次导入JSON时总是失败？

A:  这通常是UABEA的bug。建议导入失败后**立即导入第二次**，大多数情况第二次可以成功解析。

### Q: 转换后的简中/繁中文本没有效果？
A:  请检查是否：
- 导入了正确的语言文件（zh_CN 或 zh_TW）；
- 修改过的DLL是否被正确替换进游戏目录；
- 使用的字体资源是否覆盖了所有汉字；
- 批量导入贴图是否成功（必要时重新导入）。


### Q: 脚本跑完但内容没有更新？

A:  请检查：
- `translation-tools/weblate`目录下的翻译文件是否为**最新导出的版本**；
- Weblate中的中文翻译是否为空（脚本不会导入空翻译）；
- 脚本运行中是否有报错输出。