
using UnityEngine.UIElements;

public interface UIControllerInterface
{
    string MyURL { get; set; } //glasses에서 UI 생성시 기존에 생성된 UI와 동일한 페이지 인지 비교하기 위함
    TemplateContainer MyRoot { get; set; } //stack에 저장된 컨트롤러를 꺼낼시 거기에 해당되는 UI를 불러오기 위함

    void Initialize(string url, TemplateContainer root); //경로 기반으로 UI를 가져 불러올시 해당 이름과 연관된 Script를 생성하고 Initialize를 콜해 기능 매핑
}