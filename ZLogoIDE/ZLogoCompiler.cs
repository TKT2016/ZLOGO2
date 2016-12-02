using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZCompileCore.Builders;
using ZCompileCore.Loads;
using ZCompileCore.Reports;
using ZLangRT;
using ZLangRT.Descs;
using ZLangRT.Utils;
using ZLogoEngine;

namespace ZLogoIDE
{
    public class ZLogoCompiler
    {
        public const string ZLogoExt = ".zlogo";
        private ZCompileProjectModel projectModel;
        private ZCompileClassModel classModel;
        private FileInfo srcFileInfo;

        public ProjectCompileResult CompileResult { get; private set;}

        private static TKTProcDesc 开始绘图DESC;
        private static TKTProcDesc 开始画图DESC;
        private static TKTProcDesc 开始绘画DESC;

        static ZLogoCompiler()
        {
            开始绘图DESC = new TKTProcDesc();
            开始绘图DESC.Add("开始绘图");

            开始画图DESC = new TKTProcDesc();
            开始画图DESC.Add("开始画图");

            开始绘画DESC = new TKTProcDesc();
            开始绘画DESC.Add("开始绘画");
        }

        public ZLogoCompiler(FileInfo zlogoFileInfo)
        {
            srcFileInfo = zlogoFileInfo;
            projectModel = new ZCompileProjectModel();
            classModel = new ZCompileClassModel();

            projectModel.ProjectRootDirectoryInfo = srcFileInfo.Directory;
            projectModel.BinaryFileKind = PEFileKinds.Dll;
            projectModel.BinarySaveDirectoryInfo = srcFileInfo.Directory;
            projectModel.ProjectPackageName = "ZLogoIDE";
            projectModel.EntryClassName = Path.GetFileNameWithoutExtension(srcFileInfo.FullName);
            projectModel.BinaryFileNameNoEx = Path.GetFileNameWithoutExtension(srcFileInfo.FullName);     
            projectModel.AddRefPackage("Z语言系统");
            projectModel.AddRefDll(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ZLOGO开发包.dll")));
            projectModel.NeedSave = false;
            classModel.SourceFileInfo = srcFileInfo;
            classModel.PreSourceCode =
@"

使用包:ZLOGO开发包;
简略使用:颜色,补语控制;

属于:绘图窗体;

";
            projectModel.AddClass(classModel);
        }

        public ProjectCompileResult Compile()
        {
            ZCompileBuilder builder = new ZCompileBuilder();
            ProjectCompileResult result = builder.CompileProject(projectModel);
            CompileResult= result;
            CheckStartMethodEnum checkStartResult = CheckStartMethod(result);
            if (checkStartResult == CheckStartMethodEnum.NoType)
            {
                
            }
            else if (checkStartResult == CheckStartMethodEnum.NoExists)
            {
                CompileMessage compileMessage = new CompileMessage();
                compileMessage.Text = "程序中不存在'开始绘图'或者'开始画图'或者'开始绘画'过程";
                result.Errors.Add(compileMessage);
            }
            else
            {
                
            }
            return result;
        }

        public void Run()
        {
            if (CompileResult == null)
            {
                Compile();
            }
            if (CompileResult == null)
            {
                return;
            }
 
            if (CompileResult.CompiledTypes.Count > 0)
            {
                Type type = CompileResult.CompiledTypes[0];

                using (TurtleForm turtleForm = ReflectionUtil.NewInstance(type) as TurtleForm)
                {
                    turtleForm.Run();
                }
            }
        }

        static CheckStartMethodEnum CheckStartMethod(ProjectCompileResult projectCompileResult)
        {
            if (projectCompileResult.CompiledTypes.Count == 0) return CheckStartMethodEnum.NoType;
            Type type = projectCompileResult.CompiledTypes[0];
            TktGcl gcl= new TktGcl(type, null);
            TKTProcDesc searchedProcDesc = null;
            searchedProcDesc = gcl.SearchProc(开始绘图DESC);
            if (searchedProcDesc != null) return CheckStartMethodEnum.开始绘图;
            searchedProcDesc = gcl.SearchProc(开始画图DESC);
            if (searchedProcDesc != null) return CheckStartMethodEnum.开始画图;
            searchedProcDesc = gcl.SearchProc(开始绘画DESC);
            if (searchedProcDesc != null) return CheckStartMethodEnum.开始绘画;
            return CheckStartMethodEnum.NoExists;
        }

        enum CheckStartMethodEnum
        {
            NoType,
            NoExists,
            开始绘图,
            开始画图,
            开始绘画
        }
    }
}
