import os, shutil, json, zhconv
from pathlib import Path


def convert_zh_to_tw(path, format):
    for root, dirs, files in os.walk(path):
        for file in files:
            if file.endswith(format):
                input_file = os.path.join(root, file)
                output_file = os.path.join(
                    root.replace("zh_CN", "zh_TW"), file.replace("zh_CN", "zh_TW")
                )
                with open(input_file, "r", encoding="utf-8") as f:
                    content = f.read()
                content_tw = zhconv.convert(content, "zh-tw")
                # 寫入檔案
                with open(output_file, "w", encoding="utf-8") as f:
                    f.write(content_tw)


# 递归读取文件夹tmpgui中的所有JSON文件
def read_json_files(directory, textkeylist):
    translation_dict = {}  # 用来存储所有的m_text值
    translation_set = set()
    it = list([1])

    def recursive_search(obj, key):
        if isinstance(obj, dict):
            for k, v in obj.items():
                if k == key:
                    if isinstance(v, str):
                        if v not in translation_set and v != "":
                            translation_set.add(v)
                            translation_dict[
                                f"{file.replace(".json", '')}-{key}-{it[0]}"
                            ] = v
                            it[0] += 1
                    elif isinstance(v, list):
                        for item in v:
                            if (
                                isinstance(item, str)
                                and item not in translation_set
                                and item != ""
                            ):
                                translation_set.add(item)
                                translation_dict[
                                    f"{file.replace(".json", '')}-{key}-{it[0]}"
                                ] = item
                                it[0] += 1
                            else:
                                recursive_search(item, key)
                else:
                    recursive_search(v, key)
        elif isinstance(obj, list):
            for item in obj:
                recursive_search(item, key)

    # 遍历目录中的所有文件和子目录
    for root, dirs, files in os.walk(directory):
        for file in files:
            if file.endswith(".json"):
                file_path = os.path.join(root, file)
                # 读取每个json文件
                with open(file_path, "r", encoding="utf-8") as f:
                    try:
                        data = json.load(f)
                        for key in textkeylist:
                            it[0] = 1
                            recursive_search(data, key)
                    except json.JSONDecodeError:
                        print(f"文件 {file_path} 解析错误")
    return translation_dict


def save_translation_dict(translation_dict, output_file):
    with open(output_file, "w", encoding="utf-8") as f:
        json.dump(translation_dict, f, ensure_ascii=False, indent=4)


# 从新json中更新旧翻译
def update_translation_json(old_srctext, new_srctext, old_dsttext, new_dsttext):
    with open(old_srctext, "r", encoding="utf-8") as f:
        old_src = json.load(f)
    with open(new_srctext, "r", encoding="utf-8") as f:
        new_src = json.load(f)
    with open(old_dsttext, "r", encoding="utf-8") as f:
        old_dst = json.load(f)

    # 首先从old_src, old_dst建立翻译字典
    translation_dict = {}
    for key, item in old_src.items():
        if key in old_dst and old_dst[key] != "":
            translation_dict[item] = old_dst[key]
    # 然后从new_src中获取新的翻译
    dst = {}
    newtext_dict = {}
    notfound_dict = {}
    for key, item in new_src.items():
        if item in translation_dict.keys():
            dst[key] = translation_dict[item]
        else:
            dst[key] = ""
            newtext_dict[key] = item

    for item in translation_dict:
        if item not in new_src.values():
            notfound_dict[item] = translation_dict[item]

    # 将翻译写入新json
    with open(new_dsttext, "w", encoding="utf-8") as f:
        json.dump(dst, f, ensure_ascii=False, indent=4)

    # 将未找到的翻译写入json
    with open(
        new_dsttext.replace(".json", "") + "-newtext.json", "w+", encoding="utf-8"
    ) as f:
        json.dump(newtext_dict, f, ensure_ascii=False, indent=4)

    with open(
        new_dsttext.replace(".json", "") + "-notfound.json", "w+", encoding="utf-8"
    ) as f:
        json.dump(notfound_dict, f, ensure_ascii=False, indent=4)


# 将翻译导入回text
def import_translation(
    srcfolder, distfolder, textkeylist, translationsrc, translationdst
):
    shutil.copytree(srcfolder, "import_tmp")
    # 将翻译导入text，首先打开对应的json文件
    with open(translationsrc, "r", encoding="utf-8") as f:
        src = json.load(f)
    with open(translationdst, "r", encoding="utf-8") as f:
        dst = json.load(f)

    # 根据 src / dst 构建翻译字典
    transdict = dict()
    for key, item in src.items():
        if key in dst and dst[key] != "":
            transdict[item] = dst[key]
        else:
            transdict[item] = item

    # 遍历distfolder所有的json文件，使用递归来搜索对应的key
    # 遍历目录中的所有文件和子目录
    for root, _, files in os.walk("import_tmp"):
        for file in files:
            if file.endswith(".json"):
                file_path = os.path.join(root, file)
                # 读取每个json文件
                # 读取 json 文件
                with open(file_path, "r", encoding="utf-8") as f:
                    data = json.load(f)
                    # 对每个指定的 key 进行递归搜索并修改
                    for key in textkeylist:

                        def recursive_search(obj):
                            if isinstance(obj, dict):
                                for k, v in obj.items():
                                    if k == key:
                                        # 如果是字符串且存在于 transdict 中，直接修改
                                        if isinstance(v, str) and v in transdict:
                                            obj[k] = transdict[v]
                                        # 如果是列表，逐项检查
                                        elif isinstance(v, list):
                                            for idx, item in enumerate(v):
                                                if (
                                                    isinstance(item, str)
                                                    and item in transdict
                                                ):
                                                    obj[k][idx] = transdict[item]
                                                else:
                                                    recursive_search(item)
                                    else:
                                        recursive_search(v)
                            elif isinstance(obj, list):
                                for item in obj:
                                    recursive_search(item)

                        recursive_search(data)
                    with open(
                        f"{distfolder}/{os.path.basename(file_path)}",
                        "w",
                        encoding="utf-8",
                    ) as f:
                        json.dump(data, f, ensure_ascii=False, indent=2)
        shutil.rmtree("import_tmp")


if __name__ == "__main__":
    # 备份文件
    translation_key_mtext = [
        "m_text",
        "BIOSText",
        "SettingsElementName",
        "SettingsDescription",
        "ExtraName",
        "CreditNames",
        "CreditDescription",
        # 0304补充
        "GoalHint",
        "HypothesisName",
        "HypothesisTagline",
        "HypothesisDescription",
        "TimeWhenWritten",
        # 0319补充
        "LogName",
        # 0320补充
        "ItemName",
        "ItemTitle",
        "BattleInfo",
        "ItemDescription",
        "TopbarInfo",
        "KrisItemUsed_SusieDialogue",
        "KrisItemUsed_RalseiDialogue",
        "KrisItemUsed_NoelleDialogue",
        "SusieItemUsed_SusieDialogue",
        "SusieItemUsed_RalseiDialogue",
        "SusieItemUsed_NoelleDialogue",
        "RalseiItemUsed_SusieDialogue",
        "RalseiItemUsed_RalseiDialogue",
        "RalseiItemUsed_NoelleDialogue",
        "ActionName",
        "ActionDescription",
        "LargeImageText",
        "State",
        "Details",
        "VesselText",
        "ThisControlText",
        # 0327补充
        "controls",
    ]

    save_translation_dict(
        read_json_files("text/en_US", translation_key_mtext),
        "strings/mtext.json",
    )
    save_translation_dict(
        read_json_files("text/en_US", ["Text", "Array"]), "strings/dialogue.json"
    )
    update_translation_json(
        "translation-tools/weblate/dialogue/en.json",
        "strings/dialogue.json",
        "translation-tools/weblate/dialogue/zh_CN.json",
        "strings/dialogue-zh_CN.json",
    )
    update_translation_json(
        "translation-tools/weblate/mtext/en.json",
        "strings/mtext.json",
        "translation-tools/weblate/mtext/zh_CN.json",
        "strings/mtext-zh_CN.json",
    )
    copy_dict_str = {
        "strings/dialogue.json": "translation-tools/weblate/dialogue/en.json",
        "strings/mtext.json": "translation-tools/weblate/mtext/en.json",
        "strings/dialogue-zh_CN.json": "translation-tools/weblate/dialogue/zh_CN.json",
        "strings/mtext-zh_CN.json": "translation-tools/weblate/mtext/zh_CN.json",
    }
    for src, dst in copy_dict_str.items():
        os.system(f"cp -r {src} {dst}")

    import_translation(
        "text/en_US",
        "text/zh_CN",
        ["Text", "Array"],
        "translation-tools/weblate/dialogue/en.json",
        "translation-tools/weblate/dialogue/zh_CN.json",
    )

    import_translation(
        "text/zh_CN",
        "text/zh_CN",
        translation_key_mtext,
        "translation-tools/weblate/mtext/en.json",
        "translation-tools/weblate/mtext/zh_CN.json",
    )
    convert_zh_to_tw("text/zh_CN", ".json")
