❗ Project HG 프로젝트에서 제가 작성했고, 불필요한 부분은 제거한 스크립트를 올리는 리포지토리입니다. ❗

------------------------------------------------------------------------

Release date : 개발 중단

Platform : Mobile

------------------------------------------------------------------------


🛠 저는 이 게임에서 대표적으로 이런 걸 구현했습니다!

- Object pool과 적 생성 Coroutine
- 카메라 컨트롤
- 플레이어 자동 이동
- 적과 아군의 자동 이동

------------------------------------------------------------------------

🛠 Object pool과 적 생성 Coroutine

- 대량의 적이 나올 예정이었기 때문에 스테이지마다 최대로 생성될 오브젝트만큼 미리 생성시켜 둔 뒤 재활용하기 위해 작성하던 코드입니다.
- Code - [https://github.com/dydvn/Project_HG/blob/main/Manager_Play.cs](https://github.com/dydvn/Project_HG/blob/main/Manager_Play.cs)

------------------------------------------------------------------------

🛠  카메라 컨트롤


- 플레이어가 터치로 카메라를 이동시킬 수 있지만 일정 범위 이상 이동하지 못하게 구현한 코드입니다.
- Code - [https://github.com/dydvn/Project_HG/blob/main/CameraControl.cs](https://github.com/dydvn/Project_HG/blob/main/CameraControl.cs)

------------------------------------------------------------------------

🛠 플레이어 자동 이동


- 플레이어의 타깃을 지정해주면 해당 타겟에게 자동으로 이동하는 코드입니다.
- NavMesh를 이용하였습니다.
- Code - [https://github.com/dydvn/Project_HG/blob/main/Player.cs](https://github.com/dydvn/Project_HG/blob/main/Player.cs)


------------------------------------------------------------------------

🛠 적과 아군의 자동 이동

- 별도의 조작 없이 서로 가장 가까운 타깃을 향해 이동한 뒤 공격하는 코드입니다.
- LINQ 메소드와 NavMesh를 이용하였습니다.
- Code - [https://github.com/dydvn/Project_HG/blob/main/Crew.cs](https://github.com/dydvn/Project_HG/blob/main/Crew.cs)
- Code - [https://github.com/dydvn/Project_HG/blob/main/Enemy.cs](https://github.com/dydvn/Project_HG/blob/main/Enemy.cs)
