# Gitter
Gitter Bot API in C#

##Usage

      public static Gitter api;
      api = new Gitter("token", "roomID", "bot name");
      api.sendMessage("欢迎加入FCC中文社区！");
      api.doNext(async (m) =>
        string text = m.text;
      });
          
